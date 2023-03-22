using System.Data.SqlClient;
using Windows.System;

namespace ToDoTasks.Model.Connection
{
    public class MSSQLConnector
    {
        public MSSQLConnector()
        {
            _connectionBuilder = new SqlConnectionStringBuilder();
            _sqlStringModel = new MSSQLStringModel();
        }
        public MSSQLConnector(MSSQLStringModel mSSQLString) 
        {
            _sqlStringModel = mSSQLString;
            _connectionBuilder = new SqlConnectionStringBuilder();

        }
        private MSSQLStringModel _sqlStringModel;
        private SqlConnectionStringBuilder _connectionBuilder;
        public bool IsStringExists()
        {
            return (!(string.IsNullOrEmpty(_sqlStringModel.UserID) && string.IsNullOrEmpty(_sqlStringModel.Password) && string.IsNullOrEmpty(_sqlStringModel.DataSource)));
        }
        public string GetFulConnectionString()
        {
            _connectionBuilder.DataSource = $"{_sqlStringModel.DataSource}";
            _connectionBuilder.UserID = _sqlStringModel.UserID;
            _connectionBuilder.Password = _sqlStringModel.Password;
            _connectionBuilder.InitialCatalog = "ToDoList";
            _connectionBuilder.IntegratedSecurity = true;
            _connectionBuilder.TrustServerCertificate = true;
            return _connectionBuilder.ConnectionString;
        }

    }
}
