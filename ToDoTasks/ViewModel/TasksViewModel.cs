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


        public TasksViewModel()
        {
            _modelDAL = new ModelsDAL();
            _toDoTasks = _modelDAL.GetToDoTasksList();
            _toDoTask = _toDoTasks?.First() ?? new ToDoTaskModel();
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
