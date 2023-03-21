using System.Collections.ObjectModel;
using ToDoTasks;
using ToDoTasks.Model.DataOperations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows.Input;
using System;

namespace ToDoTasks.ViewModel
{
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
        private bool IsTaskExists(int id)
        {
            return ToDoTasks.Where(t => t.ID == id).Any() ? true : false;
        }
        public ICommand DeleteCommand { get; set; }
        private void ExecuteDeleteCommand(object param)
        {
            if (param != null)
            {
                int id = (int)param;
                if (IsTaskExists(id))
                {
                    _modelDAL.DeleteToDoTask(id);
                    ToDoTask = ToDoTasks.Where(t => t.ID == id).Select(t => t).Single();
                    ToDoTasks.Remove(ToDoTask);
                }
                else
                    return;
            }
            else
                return;
        }
        public void AddNewTask(Person person, string taskName, string description)
        {
            _modelDAL.InsertToDoTask(description,taskName, person.FirstName, person.LastName);
            ToDoTask = new ToDoTaskModel(ToDoTasks.Last().ID + 1,description, person.ID, taskName, person.FirstName, person.LastName);
            ToDoTasks.Add(ToDoTask);
        }

        public void UpdateTaskInDB(ToDoTaskModel model, string description)
        {
            _modelDAL.UpdateTask(model.ID, description, model.Name, model.PersonFirstName, model.PersonLastName);
            ToDoTasks.Remove(model);
            model.Description = description;
            ToDoTask = model;
            ToDoTasks.Add(ToDoTask);
        }
        public void AddNewPerson(string firstName, string lastName)
        {
            Person = new Person(Persons.Last().ID + 1, firstName, lastName);
            _modelDAL.InsertPerson(Person);
            Persons.Add(Person);
        }

        public TasksViewModel()
        {
            _modelDAL = new ModelsDAL();
            _toDoTasks = _modelDAL.GetToDoTasksList();
            _persons = _modelDAL.GetPersons();
            this.DeleteCommand = new DelegateCommand(ExecuteDeleteCommand);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }








    }
}
