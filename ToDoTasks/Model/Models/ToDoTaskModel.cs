

using System;

namespace ToDoTasks
{
    /// <summary>
    /// The base Task model class it biniding the data from the DB
    /// Also it contains the Person data info for the application [View]
    /// </summary>
    public class ToDoTaskModel
    {
        public Int64 ID { get; set; } //In SQlite is used the type of a data - INTEGER, in a C# it equals - [Int64] or [long] type.
        public string Description { get; set; }
        public Int64 AssignedPersonID { get; set; } //In SQlite is used the type of a data - INTEGER, in a C# it equals - [Int64] or [long] type.
        public string Name { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        
        public ToDoTaskModel()
        {
            
        }
        public ToDoTaskModel(Int64 id, string description, Int64 personId, string name, string personFirstName, string personLastName)
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
