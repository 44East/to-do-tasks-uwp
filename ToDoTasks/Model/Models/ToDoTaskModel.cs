using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTasks.Model.Models
{
    public class ToDoTaskModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int AssignedPersonID { get; set; }
        public string Name { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        
        public ToDoTaskModel()
        {
            
        }
        public ToDoTaskModel(int id, string description, int personId, string name, string personFirstName, string personLastName)
        {
            ID = id;
            Description = description;
            AssignedPersonID = personId;
            Name = name;
            PersonFirstName = personFirstName;
            PersonLastName = personLastName;
        }
    }
}
