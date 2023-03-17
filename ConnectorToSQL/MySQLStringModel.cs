
namespace ConnectorToSQL
{
    public class MySQLStringModel
    {
        public MySQLStringModel() { }
        public MySQLStringModel(string server, string userId, string password)
        {
            Server = server;
            UserID = userId;
            Password = password;
        }
        public string Server { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
    }
}
