using System;

namespace ToDoTasks.Model.Models
{
    /// <summary>
    /// The Data class which contains the queries for creation a new DataBase if the DB doesn't exist.
    /// </summary>
    internal static class CreationLocalDBSqlQuerys
    {
        public static string AddTestPersonsInDBQuery { get; } = $@"INSERT INTO Persons(First_Name, Last_Name) VALUES
                                                                   ('John','McLean'),
                                                                   ('Woody','Woodpecker'),
                                                                   ('Clark','Kent'),
                                                                   ('Peter','Parker')";
        public static string AddTestTasksInDBQuery { get; } = $@"INSERT INTO Tasks (Description, Assigned_Person, Name) VALUES 
                                                                 ('Tap into the tree', 2, 'Woody deals'),
                                                                 ('Delivery pizza', 4, 'Spider-Man deals'),
                                                                 ('Write an article and send it to the editor.', 3, 'Super-Man deals'),
                                                                 ('Walking on a broken glass', 1,'Die-Hard deals')";

    }
}
