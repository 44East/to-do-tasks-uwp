using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTasks.Model.Models
{
    public class ToDoTask
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int AssignedPersonID { get; set; }
        public string Name { get; set; }
        public Person AssignedPerson { get; set; }
        public ToDoTask(int id, string description, int personId, string name, Person person)
        {
            ID = id;
            Description = description;
            AssignedPerson = person;
            AssignedPersonID = personId;
            Name = name;
        }
    }
}
