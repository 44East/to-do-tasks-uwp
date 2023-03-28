using System.Collections.ObjectModel;
using ToDoTasks.Model.DataOperations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows.Input;
using System;

namespace ToDoTasks.ViewModel
{
    /// <summary>
    /// The general [ViewModel] part into the app. It contains the Persons and Tasks data which recieved from [Model]
    /// </summary>
    public class TasksViewModel : INotifyPropertyChanged
    {
        private ModelsDAL _modelDAL;
        private ToDoTaskModel _toDoTask;
        private ObservableCollection<ToDoTaskModel> _toDoTasks;
        private ObservableCollection<Person> _persons;
        private Person _person;
        public ToDoTaskModel ToDoTask
        {
            get => _toDoTask;
            set
            {
                _toDoTask = value;
                OnPropertyChanged("SelectedTask");
            }

        }
        public ObservableCollection<ToDoTaskModel> ToDoTasks
        {
            get => _toDoTasks;
            set
            {
                _toDoTasks = value;
                OnPropertyChanged("TasksList");
            }
        }
        public ObservableCollection<Person> Persons
        {
            get => _persons;
            set
            {
                _persons = value;
                OnPropertyChanged("PersonsList");
            }
        }
        public Person Person
        {
            get => _person;
            set 
            { 
                _person = value;
                OnPropertyChanged("SelectedPerson");
            }
        }

        /// <summary>
        /// Check for a task before deleting from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsTaskExists(Int64 id)
        {
            return ToDoTasks.Where(t => t.ID == id).Any() ? true : false;
        }
        
        public void DeleteTask(Int64 id)
        {
            if (IsTaskExists(id))
            {
                _modelDAL.DeleteToDoTask(id);
                ToDoTask = ToDoTasks.Where(t => t.ID == id).Select(t => t).Single();
                ToDoTasks.Remove(ToDoTask);
            }
            else
                return;
        }
        /// <summary>
        /// Check the data from the DB before listing it for a MainPage 
        /// </summary>
        /// <returns></returns>
        public bool IsDataExist()
        {
            return (Persons.Count > 0 && ToDoTasks.Count > 0) ? true : false;
        }
        /// <summary>
        /// Send a new ToDoTask data into a [Model] part for adding it into the DB 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="taskName"></param>
        /// <param name="description"></param>
        public void AddNewTask(Person person, string taskName, string description)
        {
            _modelDAL.InsertToDoTask(description,taskName, person);
            ToDoTask = new ToDoTaskModel(ToDoTasks.Last().ID + 1,description, person.ID, taskName, person.FirstName, person.LastName);
            ToDoTasks.Add(ToDoTask);
        }
        /// <summary>
        /// Send a new ToDoTask description into a [Model] part for updating the ToDaTask data into the DB
        /// </summary>
        /// <param name="model"></param>
        /// <param name="description"></param>
        public void UpdateTaskInDB(ToDoTaskModel model, string description)
        {
            _modelDAL.UpdateTask(model, description);
            ToDoTasks.Remove(model);
            model.Description = description;
            ToDoTask = model;
            ToDoTasks.Add(ToDoTask);
        }
        /// <summary>
        /// Send a new Person data into a [Model] part for adding it into the DB
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public void AddNewPerson(string firstName, string lastName)
        {
            Person = new Person(Persons.Last().ID + 1, firstName, lastName);
            _modelDAL.InsertPerson(Person);
            Persons.Add(Person);
        }

        public TasksViewModel()
        {
            _modelDAL = new ModelsDAL();
            Persons = _modelDAL.GetPersons();
            ToDoTasks = _modelDAL.GetToDoTasksList();
        }
        /// <summary>
        /// Notification property for the [View] part the app
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }








    }
}
