using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTasks
{
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
