using Microsoft.Data.SqlClient;

namespace ConnectorToSQL
{
    public class MSSQLConnector
    {
        public MSSQLConnector() 
        { 
            _connectionBuilder = new SqlConnectionStringBuilder();
            _fileWorker = new FileWorker<MSSQLStringModel> ();
            _sqlStringModel = _fileWorker.GetModelFromFile("mssql-data-src.json");
        }
        private FileWorker<MSSQLStringModel> _fileWorker;
        private readonly MSSQLStringModel _sqlStringModel;
        private SqlConnectionStringBuilder _connectionBuilder;
        public string GetFulConnectionString()
        {
            _connectionBuilder.InitialCatalog = "ToDoList";
            _connectionBuilder.DataSource = $"{_sqlStringModel.DataSource}";
            _connectionBuilder.IntegratedSecurity = true;
            _connectionBuilder.TrustServerCertificate = true;
            _connectionBuilder.UserID = _sqlStringModel.UserID;
            _connectionBuilder.Password = _sqlStringModel.Password;
            return _connectionBuilder.ConnectionString;
        }
        public void InsertConnectionData(string server, string userId, string password)
        {
            var model = new MSSQLStringModel(server, userId, password);
            _fileWorker.InsertModelInFile(model, "mssql-data-src.json");
        }

    }
}
