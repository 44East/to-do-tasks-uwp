using ConnectorToSQL;
namespace DataConnectionInjection
{
    internal class Program
    {

        static void Main(string[] args)
        {
            bool isAnswerCorrect = false;
            do
            {
                Console.WriteLine("Select DB [MSSQL - 1] / [MySQL - 2]");
                var answer = Console.ReadLine()?.Trim();
                switch (answer)
                {
                    case "1":
                        isAnswerCorrect = true;
                        InsertDataForMSSQL();
                        break;
                    case "2":
                        isAnswerCorrect = true;
                        InsertDataForMySQL();
                        break;
                    default:
                        Console.WriteLine("Try again!");
                        break;
                }
            } while (!isAnswerCorrect);
        }
        private static void InsertDataForMSSQL()
        {
            MSSQLConnector mssql = new MSSQLConnector();
            Console.Write("Data source:");
            var dataSrc = Console.ReadLine()?.Trim();
            Console.Write("User ID:");
            var userId = Console.ReadLine()?.Trim();
            Console.Write("Password:");
            var password = Console.ReadLine()?.Trim();
            mssql.InsertConnectionData(dataSrc, userId, password);

        }
        private static void InsertDataForMySQL()
        {
            MySQLConnector mssql = new MySQLConnector();
            Console.Write("Server:");
            var dataSrc = Console.ReadLine()?.Trim();
            Console.Write("User ID:");
            var userId = Console.ReadLine()?.Trim();
            Console.Write("Password:");
            var password = Console.ReadLine()?.Trim();
            mssql.InsertConnectionData(dataSrc, userId, password);
        }
    }

}