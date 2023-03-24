

namespace ToDoTasks
{
    /// <summary>
    /// The base Person model for binding data from the DB
    /// 
    /// </summary>
    public class Person
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public Person()
        {
            
        }
        public Person(int id, string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = id;
            FullName = FirstName + " " + LastName;
        }
    }
}
