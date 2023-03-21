using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoTasks.Model.Models;
using System.Collections.ObjectModel;
using ToDoTasks.Model.Connection;
using Windows.UI.Xaml;
using Windows.Networking;

namespace ToDoTasks.Model.DataOperations
{
    public class ModelsDAL : IDisposable
    {
        private MSSQLConnector _connector;
        private readonly string _connectionString;
        private bool _disposed;
        private SqlConnection _sqlConnection = null;
        public ModelsDAL()
        {
            _connector = new MSSQLConnector();
            _connectionString = @"Data Source=DESKTOP-2J66JK5\MSSQLSERVER22;User ID=User;Password=1234567890;Integrated Security=true;Initial Catalog=ToDoList;TrustServerCertificate=true;";//_connector.GetFulConnectionString();
        }
        private void OpenConnection()
        {
            _sqlConnection = new SqlConnection()
            {
                ConnectionString = _connectionString
            };
            _sqlConnection.Open();
        }
        private void CloseConnection()
        {
            if (_sqlConnection.State != ConnectionState.Closed)
            {
                _sqlConnection.Close();
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) _sqlConnection.Dispose();
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public ObservableCollection<ToDoTaskModel> GetToDoTasksList()
        {
            OpenConnection();
            var toDoList = new ObservableCollection<ToDoTaskModel>();
            var sql = $@"SELECT 
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
        public ObservableCollection<Person> GetPersons()
        {
            OpenConnection();
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
        public void InsertToDoTask(string description, string taskName, string firstName, string lastName)
        {
            OpenConnection();
            var sql = $@"DECLARE @PersonID INT
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
        public void DeleteToDoTask(int id)
        {
            OpenConnection();
            var sql = $@"DELETE FROM Tasks WHERE ID = {id}";
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
        public void InsertPerson(Person person)
        {
            OpenConnection();
            var sql = $@"INSERT Persons(First_Name, Last_Name) VALUES
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
        public void UpdateTask(int id, string description, string taskName, string firstName, string lastName)
        {
            OpenConnection();
            var sql = $@"DECLARE @PersonID INT
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
}
