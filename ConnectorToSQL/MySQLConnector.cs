using MySql.Data.MySqlClient;

namespace ConnectorToSQL
{
    public class MySQLConnector
    {
        public MySQLConnector()
        {
            _connectionString = new MySqlConnectionStringBuilder();
            _fileWorker = new FileWorker<MySQLStringModel>();
            //_stringModel = _fileWorker.GetModelFromFile("mysql-data-src.json");
        }
        private FileWorker<MySQLStringModel> _fileWorker;
        private MySqlConnectionStringBuilder _connectionString;
        private MySQLStringModel _stringModel;
        public string GetFulConnectionString()
        {
            _connectionString.Database = "ToDoList";
            _connectionString.Server = $"{_stringModel.Server}";
            _connectionString.IntegratedSecurity = true;
            _connectionString.UserID = _stringModel.UserID;
            _connectionString.Password = _stringModel.Password;
            return _connectionString.ConnectionString;
        }
        public void InsertConnectionData(string server, string userId, string password)
        {
            var model = new MySQLStringModel(server, userId, password);
            _fileWorker.InsertModelInFile(model, "mysql-data-src.json");
        }
    }
}
