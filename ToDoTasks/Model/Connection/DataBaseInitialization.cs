using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using ToDoTasks.Model.Models;
using Windows.Storage;
using Windows.UI.Xaml.Controls.Primitives;

namespace ToDoTasks.Model.Connection
{
    internal class DataBaseInitialization
    {
        private bool _isDataBaseFileExists;
        private bool _isDataBaseDataExists;
        private string _dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "toDoList.db");
        public DataBaseInitialization()
        {
            _isDataBaseFileExists = File.Exists(_dbPath);
            _isDataBaseDataExists = IsDataInDBExists();

        }
        public bool IsDataBaseInitialise()
        {
            var isAllDone = false;
            if (!_isDataBaseFileExists)
            {
                CreateLocalDBFile();
                CreateTableInDatabase();
                FillingDBTestContent();
                return true;
            }
            if (!_isDataBaseDataExists)
            {
                CreateTableInDatabase();
                FillingDBTestContent();
                return true;
            }
            if(_isDataBaseDataExists && _isDataBaseFileExists)
            {
                return true;
            }
            return isAllDone;
        }
        private void CreateLocalDBFile()
        {
            ApplicationData.Current.LocalFolder.CreateFileAsync("toDoList.db", CreationCollisionOption.OpenIfExists).GetResults();
        }
        private bool IsDataInDBExists()
        {
            string sql = $@"SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'Persons'";
            
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbPath}"))
            {
                db.Open();

                using (SqliteCommand command = new SqliteCommand(sql, db))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetValue(0) is string;
                        }
                    }
                }

            }
            return false;
        }
        private void CreateTableInDatabase()
        {
            string onForeignKey = "PRAGMA foreign_keys = ON;";

            string taskTableCommand = $@"CREATE TABLE IF NOT EXISTS Tasks (
                                             ID INTEGER PRIMARY KEY AUTOINCREMENT, 
                                             Description NVARCHAR(200) NOT NULL,
                                             Assigned_Person INTEGER NOT NULL,
                                             Name NVARCHAR(30) NOT NULL,
                                             FOREIGN KEY (Assigned_Person) 
                                             REFERENCES Persons(ID))";

            string personsTableCommand = $@"CREATE TABLE IF NOT EXISTS Persons (
                                                ID INTEGER PRIMARY KEY AUTOINCREMENT, 
                                                First_Name NVARCHAR(30) NOT NULL,
                                                Last_Name NVARCHAR(30) NOT NULL)";
            Stack<string> sqlQueries = new Stack<string>();
            sqlQueries.Push(taskTableCommand);
            sqlQueries.Push(personsTableCommand);
            sqlQueries.Push(onForeignKey);

            using (SqliteConnection db = new SqliteConnection($"Filename={_dbPath}"))
            {
                db.Open();
                while (sqlQueries.Count > 0)
                {
                    using (SqliteCommand command = new SqliteCommand(sqlQueries.Pop(), db))
                    {
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        private void FillingDBTestContent()
        {
            var queriesStack = new Stack<string>();
            queriesStack.Push(CreationLocalDBSqlQuerys.AddTestTasksInDBQuery);
            queriesStack.Push(CreationLocalDBSqlQuerys.AddTestPersonsInDBQuery);
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbPath}"))
            {
                do
                {
                    using (SqliteCommand command = new SqliteCommand(queriesStack.Pop(), db))
                    {
                        try
                        {
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                        catch (SqliteException ex)
                        {
                            Exception error = new Exception("LocalBD cann't fill!", ex);
                            throw error;
                        }
                    }
                } while (queriesStack.Count > 0);
            }
        }
    }
}
