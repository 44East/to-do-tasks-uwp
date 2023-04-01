using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using ToDoTasks.Model.Connection;
using ToDoTasks.Model.Models;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace ToDoTasks.Model.DataOperations
{
    /// <summary>
    /// The key class in the Model of the application. It aggregates all methods for operations with the data from DB(MSSQL Server).
    /// It uses the ADO.NET tecnology for the CRUD operations. 
    /// </summary>
    public class ModelsDAL : IDisposable
    {
        private DataBaseInitialization _initialization;
        private string _connectionString;
        private bool _disposed;
        private SqliteConnection _sqliteConnection = null;
        private bool _dbDataExists;
        public ModelsDAL()
        {
            _initialization = new DataBaseInitialization();
            _dbDataExists = _initialization.IsDataBaseInitialise();
            if(_dbDataExists)
                _connectionString = _initialization.DatabasePath;
        }
        /// <summary>
        /// Connect to the SQLite DB
        /// </summary>
        private void OpenConnection()
        {
            if (_connectionString == null)
                return;
            _sqliteConnection = new SqliteConnection($"Filename={_connectionString}");
            _sqliteConnection.Open();
        }
        /// <summary>
        /// Closing connection to the SQLite DB
        /// </summary>
        private void CloseConnection()
        {
            if (_sqliteConnection.State != ConnectionState.Closed)
            {
                _sqliteConnection.Close();
            }
        }
        /// <summary>
        /// Checks the SqlConnection to exist, if it exists, method get rid of the SqlConnection
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) _sqliteConnection.Dispose();
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
        /// Returns the ToDoTasks collection from the DB and it includes a Persons names for the ToDoTask model.
        /// </summary>
        public ObservableCollection<ToDoTaskModel> GetToDoTasksList()
        {
            OpenConnection();
            if (_sqliteConnection == null)
                return new ObservableCollection<ToDoTaskModel>();
            var toDoList = new ObservableCollection<ToDoTaskModel>();
            var sql = $@"SELECT 
                        i.ID, 
                        i.Description, 
                        i.Assigned_Person,
                        i.Name,
                        m.First_Name, 
                        m.Last_Name 
                        FROM Tasks i inner JOIN Persons m on m.ID = i.Assigned_Person";

            using (SqliteCommand command = new SqliteCommand(sql, _sqliteConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        toDoList.Add(new ToDoTaskModel(
                            (Int64)reader["ID"],
                            (string)reader["Description"],
                            (Int64)reader["Assigned_Person"],
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
            if (_sqliteConnection == null)
                return new ObservableCollection<Person>();
            var personList = new ObservableCollection<Person>();
            var sql = $@"SELECT * FROM [Persons] LIMIT 1000";
            using (SqliteCommand command = new SqliteCommand(sql, _sqliteConnection))
            {
                //command.CommandType = CommandType.Text;
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        personList.Add(new Person(
                            (Int64)reader["ID"],
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
        /// <param name="person"></param>
        public void InsertToDoTask(string description, string taskName, Person person)
        {
            OpenConnection();
            if (_sqliteConnection == null)
                return;
            var sql = $@"INSERT INTO Tasks(Description, Assigned_Person, Name) VALUES
                         ('{description}', {person.ID}, '{taskName}')";
            using (SqliteCommand command = new SqliteCommand(sql, _sqliteConnection))
            {
                SqliteTransaction transaction = null;
                try
                {
                    transaction = _sqliteConnection.BeginTransaction();
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
        public void DeleteToDoTask(Int64 id)
        {
            OpenConnection();
            if (_sqliteConnection == null)
                return;
            var sql = $@"DELETE FROM Tasks WHERE ID = {id}";
            using (SqliteCommand command = new SqliteCommand(sql, _sqliteConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
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
            if (_sqliteConnection == null)
                return;
            var sql = $@"INSERT INTO Persons (First_Name, Last_Name) VALUES
                         ('{person.FirstName}', '{person.LastName}')";
            using (SqliteCommand command = new SqliteCommand(sql, _sqliteConnection))
            {
                SqliteTransaction transaction = null;
                try
                {
                    transaction = _sqliteConnection.BeginTransaction();
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
        public void UpdateTask(ToDoTaskModel task, string description)
        {
            OpenConnection();
            if (_sqliteConnection == null)
                return;
            var sql = $@"UPDATE Tasks SET Description = '{description}', Assigned_Person = {task.AssignedPersonID}, Name = '{task.Name}'  WHERE ID = {task.ID}";
            using (SqliteCommand command = new SqliteCommand(sql, _sqliteConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
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
