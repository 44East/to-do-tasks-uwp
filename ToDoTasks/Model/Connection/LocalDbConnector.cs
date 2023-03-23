using System;
using System.Data.SqlClient;

namespace ToDoTasks.Model.Connection
{
    internal class LocalDbConnector
    {
        private SqlConnectionStringBuilder _sqlConnectionStringBuilder;
        public LocalDbConnector()
        {
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
        }
        public string GetLocalConnectionString()
        {
            _sqlConnectionStringBuilder.DataSource = $@"(localdb)\MSSQLLocalDB";
            _sqlConnectionStringBuilder.IntegratedSecurity = true;
            _sqlConnectionStringBuilder.InitialCatalog = "master";
            _sqlConnectionStringBuilder.ConnectTimeout = 30;
            _sqlConnectionStringBuilder.Encrypt = false;
            _sqlConnectionStringBuilder.TrustServerCertificate = true;
            _sqlConnectionStringBuilder.ApplicationIntent = ApplicationIntent.ReadWrite;
            _sqlConnectionStringBuilder.MultiSubnetFailover = false;
            return _sqlConnectionStringBuilder.ConnectionString;
        }
       

    }
}
