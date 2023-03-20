namespace ToDoTasks.Model.Connection
{
    public class MSSQLStringModel
    {
        public MSSQLStringModel() { }
        public MSSQLStringModel(string dataSource, string userId, string password)
        {
            DataSource = dataSource;
            UserID = userId;
            Password = password;
        }
        public string DataSource { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
    }
}
