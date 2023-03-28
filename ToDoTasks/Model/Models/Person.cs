

using System;

namespace ToDoTasks
{
    /// <summary>
    /// The base Person model for binding data from the DB
    /// 
    /// </summary>
    public class Person
    {
        public Int64 ID { get; set; } //In SQlite is used the type of a data - INTEGER, in a C# it equals - [Int64] or [long] type.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public Person()
        {
            
        }
        public Person(Int64 id, string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = id;
            FullName = FirstName + " " + LastName;
        }
    }
}
