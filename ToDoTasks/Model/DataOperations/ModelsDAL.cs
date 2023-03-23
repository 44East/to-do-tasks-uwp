﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using ToDoTasks.Model.Connection;
using ToDoTasks.Model.Models;
using System.Collections.Generic;

namespace ToDoTasks.Model.DataOperations
{
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
            if (!_dbDataExists)
                FillingDBTestContent();

        }
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
        //All methods (below) for CRUD processes use the ADO.NET connection type
        #region CRUD Operations
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
