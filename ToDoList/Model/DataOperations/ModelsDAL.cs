using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Model.Models;

using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlDataReader = Microsoft.Data.SqlClient.SqlDataReader;
using ConnectorToSQL;

namespace ToDoList.Model.DataOperations
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
            _connectionString = _connector.GetFulConnectionString();
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
        public IEnumerable<ToDoTask> ToDoList()
        {
            OpenConnection();
            var toDoList = new List<ToDoTask>();
            var sql = $@"SELECT 
                        i.ID, 
                        i.Description, 
                        i.Assigned_Person, 
                        m.First_Name, 
                        m.Last_Name 
                        FROM Tasks i inner join Persons m on m.ID = i.Maker";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        toDoList.Add(new ToDoTask(
                            (int)reader["ID"],
                            (string)reader["Description"],
                            (int)reader["Assigned_Person"],
                            new Person(
                                (string)reader["First_Name"],
                                (string)reader["Last_Name"])


                        ));
                    }
                }
            }
            return toDoList;
        }
        public void InsertToDoTask()
    }
}
