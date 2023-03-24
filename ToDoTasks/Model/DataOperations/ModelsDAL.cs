using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using ToDoTasks.Model.Connection;
using ToDoTasks.Model.Models;
using System.Collections.Generic;

namespace ToDoTasks.Model.DataOperations
{
    /// <summary>
    /// The key class in the Model of the application. It aggregates all methods for operations with the data from DB(MSSQL Server).
    /// It uses the ADO.NET tecnology for the CRUD operations. 
    /// </summary>
    public class ModelsDAL : IDisposable
    {
        private LocalDbConnector _localConnector;
        private string _connectionString;
        private bool _disposed;
        private SqlConnection _sqlConnection = null;
        private bool _dbDataExists;
        public ModelsDAL()
        {
            _localConnector = new LocalDbConnector();
            _connectionString = _localConnector.GetLocalConnectionString();
            _dbDataExists = IsTheLocalDBExists();
            if (!_dbDataExists)//If the DB doesn't exist, will be create a new test DB with the test data.
                FillingDBTestContent();

        }
        /// <summary>
        /// Connect to the MSSQL DB
        /// </summary>
        private void OpenConnection()
        {
            if (_connectionString == null)
                return;
            _sqlConnection = new SqlConnection()
            {
                ConnectionString = _connectionString
            };
            _sqlConnection.Open();
        }
        /// <summary>
        /// Closing connection to the MSSQL DB 
        /// </summary>
        private void CloseConnection()
        {
            if (_sqlConnection.State != ConnectionState.Closed)
            {
                _sqlConnection.Close();
            }
        }
        /// <summary>
        /// Checks the SqlConnection to exist, if it exists, method get rid of the SqlConnection
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) _sqlConnection.Dispose();
            _disposed = true;
        }
        /// <summary>
        /// Disposing of the SqlConnection and Finalize [this.instance] 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //All methods (below) for CRUD processes use the ADO.NET technology
        #region CRUD Operations

        /// <summary>
        /// Checks the localhost MSSQL Server DB for an availability saved database [ToDoList]
        /// It sends query to the DB for receiving an Id number of one the tables from the DB.
        /// If the DB returns null, will be create a new test DB with the test data.
        /// </summary>
        private bool IsTheLocalDBExists()
        {
            OpenConnection();
            var isDBExists = false;
            var sql = $@"SELECT OBJECT_ID (N'ToDoList.dbo.Persons', N'U') AS 'ID'";           
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        isDBExists = reader["ID"] is System.Int32;                        
                    }
                }
            }
            CloseConnection();           

            return isDBExists;
        }
        /// <summary>
        /// Method for filling the DB of the testing data. 
        /// It uses the saved qureies from the special class [CreationLocalDBSqlQuerys].
        /// Requests are sent to the database in turn using Stack.Pop()
        /// </summary>
        private void FillingDBTestContent()
        {
            var queriesStack = new Stack<string>();
            queriesStack.Push(CreationLocalDBSqlQuerys.AddTestTasksInDBQuery);
            queriesStack.Push(CreationLocalDBSqlQuerys.AddTestPersonsInDBQuery);
            queriesStack.Push(CreationLocalDBSqlQuerys.CreationTasksTableQuery);
            queriesStack.Push(CreationLocalDBSqlQuerys.CreationPersonsTableQuery);
            queriesStack.Push(CreationLocalDBSqlQuerys.DBCreationQuery);
            OpenConnection();
            do
            {
                using (SqlCommand command = new SqlCommand(queriesStack.Pop(), _sqlConnection))
                {
                    try
                    {
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Exception error = new Exception("LocalBD cann't fill!", ex);
                        throw error;
                    }
                }
            }while (queriesStack.Count > 0);
            CloseConnection();
        }
        /// <summary>
        /// Returns the ToDoTasks collection from the DB and it includes a Persons names for the ToDoTask model.
        /// </summary>
        public ObservableCollection<ToDoTaskModel> GetToDoTasksList()
        {
            OpenConnection();
            if (_sqlConnection == null)
                return new ObservableCollection<ToDoTaskModel>();
            var toDoList = new ObservableCollection<ToDoTaskModel>();
            var sql = $@"USE ToDoList
                        SELECT 
                        i.ID, 
                        i.Description, 
                        i.Assigned_Person,
                        i.Name,
                        m.First_Name, 
                        m.Last_Name 
                        FROM Tasks i inner JOIN Persons m on m.ID = i.Assigned_Person";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        toDoList.Add(new ToDoTaskModel(
                            (int)reader["ID"],
                            (string)reader["Description"],
                            (int)reader["Assigned_Person"],
                            (string)reader["Name"],
                            (string)reader["First_Name"],
                            (string)reader["Last_Name"]
                        ));
                    }
                }
            }

            CloseConnection();
            return toDoList;
        }
        /// <summary>
        /// It returns the Persons collection from the DB.
        /// </summary>
        public ObservableCollection<Person> GetPersons()
        {
            OpenConnection();
            if (_sqlConnection == null)
                return new ObservableCollection<Person>();
            var personList = new ObservableCollection<Person>();
            var sql = $@"SELECT TOP (1000) [ID],
                         [First_Name],
                         [Last_Name]
                         FROM [ToDoList].[dbo].[Persons]";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        personList.Add(new Person(
                            (int)reader["ID"],
                            (string)reader["First_Name"],
                            (string)reader["Last_Name"]
                        ));
                    }
                }
            }
            CloseConnection();
            return personList;

        }
        /// <summary>
        /// It method insert into DB NEW ToDoTask.
        /// It receives params of the Person and the ToDoTask then it will create dependences between the Person table and the Task table
        /// </summary>
        /// <param name="description"></param>
        /// <param name="taskName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public void InsertToDoTask(string description, string taskName, string firstName, string lastName)
        {
            OpenConnection();
            if (_sqlConnection == null)
                return;
            var sql = $@"USE ToDoList
                         DECLARE @PersonID INT
                         SELECT @PersonID = i.ID FROM Persons i WHERE i.First_Name = N'{firstName}' and i.Last_Name = N'{lastName}'
                         INSERT Tasks(Description, Assigned_Person, Name) VALUES
                         (N'{description}', @PersonID, N'{taskName}')";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    transaction?.Rollback();
                    throw ex;
                }
                transaction?.Commit();
            }
            CloseConnection();
        }
        /// <summary>
        /// Delete the ToDoTask in the DB by [ID] params
        /// </summary>
        /// <param name="id"></param>
        public void DeleteToDoTask(int id)
        {
            OpenConnection();
            if (_sqlConnection == null)
                return;
            var sql = $@"USE ToDoList DELETE FROM Tasks WHERE ID = {id}";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Task couldn’t be remove from DataBase!", ex);
                    throw error;
                }
            }
            CloseConnection();
        }
        /// <summary>
        /// Add NEW Person in the Person table into DB
        /// </summary>
        /// <param name="person"></param>
        public void InsertPerson(Person person)
        {
            OpenConnection();
            if (_sqlConnection == null)
                return;
            var sql = $@"USE ToDoList INSERT Persons(First_Name, Last_Name) VALUES
                         (N'{person.FirstName}', N'{person.LastName}')";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = _sqlConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    transaction?.Rollback();
                    throw ex;
                }
                transaction?.Commit();
            }
            CloseConnection();
        }
        /// <summary>
        /// Update the exist ToDoTask into DB, It updates only a [Description] field
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="taskName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public void UpdateTask(int id, string description, string taskName, string firstName, string lastName)
        {
            OpenConnection();
            if (_sqlConnection == null)
                return;
            var sql = $@"USE ToDoList DECLARE @PersonID INT
                         SELECT @PersonID = i.ID FROM Persons i WHERE i.First_Name = N'{firstName}' and i.Last_Name = N'{lastName}'
                         UPDATE Tasks SET Description = N'{description}', Assigned_Person = @PersonID, Name = N'{taskName}'  WHERE ID = {id}";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("The operation couldn’t be completed!", ex);
                    throw error;
                }
            }
            CloseConnection();
        }
    }
    #endregion
}
