using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ToDoTasks.Model.Models;
using Windows.Storage;

namespace ToDoTasks.Model.Connection
{
    /// <summary>
    /// Class for initialization a local DB and if the DB doesn't exist, this class creates the DB and filling it.
    /// </summary>
    internal class DataBaseInitialization
    {
        private bool _isDataBaseFileExists;
        private bool _isDataBaseDataExists;
        private string _dbPath;
        public string DatabasePath => _dbPath;
        
        public DataBaseInitialization()
        {
            _dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "ToDoList.db");
            _isDataBaseFileExists = File.Exists(_dbPath);
            _isDataBaseDataExists = IsDataInDBExists();

        }
        /// <summary>
        /// This method check existing of the FIle and DataBase data, and they it absent method creates a new instance of the DB by the helping methods
        /// </summary>
        /// <returns></returns>
        public bool IsDataBaseInitialise()
        {
            
            if (!_isDataBaseFileExists)
            {//If the file doesn't exist it does the full creation process
                CreateLocalDBFile();
                CreateTableInDatabase();
                FillingDBTestContent();
                _isDataBaseFileExists = true;
                _isDataBaseDataExists = true;
            }
            if (!_isDataBaseDataExists)
            {//If the file exist but the data absent, it filling the test data in the DB
                CreateTableInDatabase();
                FillingDBTestContent();
                _isDataBaseDataExists = true;
            }
            if(_isDataBaseDataExists && _isDataBaseFileExists)
            {//If the all states is true, it return "ready to go" by boolean status.
                return true;
            }
            return false;
        }
        /// <summary>
        /// Creates the DataBase local file in the User/AppData/Local/Packages/{Apllication} folder on a System disk
        /// </summary>
        private void CreateLocalDBFile()
        {
            ApplicationData.Current.LocalFolder.CreateFileAsync("ToDoList.db", CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();
        }
        /// <summary>
        /// Checks the local file SQLite DB for an availability saved database [ToDoList]
        /// It sends query to the DB for receiving a name of one the tables from the DB.
        /// If the DB returns null,it returns false.
        /// </summary>
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
        /// <summary>
        /// Creates the general tables in DataBase 
        /// </summary>
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

            //It uses the Stack<T> for creation process, it could be an other collection or some SqliteCommand(s) for an each creation table query.
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
        /// <summary>
        /// Method for filling the DB of the testing data. 
        /// It uses the saved qureies from the special class [CreationLocalDBSqlQuerys].
        /// Requests are sent to the database in turn using Stack.Pop()
        /// </summary>
        private void FillingDBTestContent()
        {
            var queriesStack = new Stack<string>();
            //It uses the Stack<T> for creation process, it could be an other collection or some SqliteCommand(s) for an each insert data query.
            queriesStack.Push(CreationLocalDBSqlQuerys.AddTestTasksInDBQuery);
            queriesStack.Push(CreationLocalDBSqlQuerys.AddTestPersonsInDBQuery);
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbPath}"))
            {
                db.Open();
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
