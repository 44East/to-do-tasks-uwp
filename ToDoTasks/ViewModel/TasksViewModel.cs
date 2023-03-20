using System.Collections.ObjectModel;
using ToDoTasks.Model.Models;
using ToDoTasks.Model.DataOperations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoTasks.ViewModel
{
    public class TasksViewModel : INotifyPropertyChanged
    {
        private ModelsDAL _modelDAL;
        private ToDoTask _toDoTask;
        private ObservableCollection<ToDoTask> _toDoTasks;
        public ToDoTask ToDoTask
        {
            get => _toDoTask;
            set
            {
                _toDoTask = value;
                OnPropertyChanged("SelectedTask");
            }

        }

        public ObservableCollection<ToDoTask> ToDoTasks
        {
            get => _toDoTasks;
            set
            {
                _toDoTasks = value;
                OnPropertyChanged("ToDoTasksList");
            }
        }


        public TasksViewModel()
        {
            _modelDAL = new ModelsDAL();
            _toDoTasks = _modelDAL.GetToDoTasksList();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }








    }
}
