

namespace ToDoTasks
{
    /// <summary>
    /// The base Task model class it biniding the data from the DB
    /// Also it contains the Person data info for the application [View]
    /// </summary>
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
