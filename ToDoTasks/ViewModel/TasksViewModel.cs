using System.Collections.ObjectModel;
using ToDoTasks.Model.Models;
using ToDoTasks.Model.DataOperations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace ToDoTasks.ViewModel
{
    public class TasksViewModel : INotifyPropertyChanged
    {
        private ModelsDAL _modelDAL;
        private ToDoTaskModel _toDoTask;
        private ObservableCollection<ToDoTaskModel> _toDoTasks;
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
                OnPropertyChanged("ToDoTasksList");
            }
        }


        public TasksViewModel()
        {
            _modelDAL = new ModelsDAL();
            _toDoTasks = _modelDAL.GetToDoTasksList();
            _toDoTask = _toDoTasks?.First() ?? new ToDoTaskModel();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }








    }
}
