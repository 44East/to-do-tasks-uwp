using System.Data.SqlClient;

namespace ToDoTasks.Model.Connection
{
    /// <summary>
    ///This class builds a connection string to the MS SQL Server.
    ///The app uses the[localhost] type connection to DB.
    /// </summary>
    internal class LocalDbConnector
    {
        private SqlConnectionStringBuilder _sqlConnectionStringBuilder;
        public LocalDbConnector()
        {
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
        }
        /// <summary>
        /// It returns the completed string for the connection
        /// </summary>
        public string GetLocalConnectionString()
        {
            _sqlConnectionStringBuilder.DataSource = $@"localhost";
            _sqlConnectionStringBuilder.IntegratedSecurity = true;
            _sqlConnectionStringBuilder.TrustServerCertificate = true;
            return _sqlConnectionStringBuilder.ConnectionString;
        }
       

    }
}
