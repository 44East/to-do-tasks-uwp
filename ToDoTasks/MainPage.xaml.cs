using System.Collections.Generic;
using System.Collections.ObjectModel;
using ToDoTasks.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ToDoTasks
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Enum for showing points in the Menu
        public enum MenuStats
        {
            Add_new_Task,
            Add_new_Person
        }
        public List<MenuStats> MenuStatsPoints { get; private set; } = new List<MenuStats>() { MenuStats.Add_new_Task, MenuStats.Add_new_Person};
        private TasksViewModel _taskViewModel;
        public ObservableCollection<ToDoTaskModel> ToDoTasks { get; set; }
        public ObservableCollection<Person> Persons { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            this._taskViewModel = new TasksViewModel();
            this.DataContext = this._taskViewModel;
            ToDoTasks = this._taskViewModel.ToDoTasks;
            Persons = this._taskViewModel.Persons;
            TasksList.ItemsSource = this.ToDoTasks;
            MenuBox.ItemsSource = this.MenuStatsPoints;
            PersonsList.ItemsSource = this.Persons;

            
        }
        
        //Get info from selected task
        private void ToDoTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToDoTaskModel taskModel = (ToDoTaskModel)TasksList.SelectedItem;
            if (taskModel != null)
            {
                HeaderTaskInfo.Visibility = Visibility.Visible;
                TaskInfoData.Visibility = Visibility.Visible;
                TextBlock_FName.Text = taskModel.PersonFirstName + " " + taskModel.PersonLastName;
                TextBlock_Description.Text = taskModel.Description;
            }
        }
        //Check Data and Connection status and get relevant script
        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_taskViewModel.IsDataExist())
            {
                switch (MenuBox.SelectedItem)
                {
                    case MenuStats.Add_new_Task:
                        MenuBoxAddTask_SelectionChanged(sender, e);
                        break;
                    case MenuStats.Add_new_Person:
                        MenuBoxAddPerson_SelectionChanged(sender, e);
                        break;
                }
            }
            else
            {
                switch (MenuBox.SelectedItem)
                {
                    case MenuStats.Add_new_Person:
                        MenuBoxAddPerson_SelectionChanged(sender, e);
                        break;
                }
            }
        }
        private void MenuBoxAddTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddNewTask.Visibility = Visibility.Visible;
            PersonsList.Visibility = Visibility.Visible;
            MenuBox.Visibility = Visibility.Collapsed;
        }
        private void MenuBoxAddPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddNewPerson.Visibility = Visibility.Visible;
            InsertFirstPersonName.Visibility = Visibility.Visible;
            InsertLastPersonName.Visibility = Visibility.Visible;
            SavePerson.Visibility = Visibility.Visible;
            AddPeronBlock.Visibility = Visibility.Visible;
            MenuBox.Visibility = Visibility.Collapsed;
        }
        //Select actual person for create task
        private void Persons_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {            
            TaskDescription.Visibility = Visibility.Visible;
            TaskName.Visibility = Visibility.Visible;
            SaveTask.Visibility = Visibility.Visible;
        }
        //Save created task
        private void SaveButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var person = (Person)PersonsList.SelectedItem;
            var taskName = TaskName.Text;
            var description  = TaskDescription.Text;
            if (!(string.IsNullOrEmpty(taskName) && string.IsNullOrEmpty(description)))
            {
                _taskViewModel.AddNewTask(person, taskName, description);
            }
            TaskName.Text = string.Empty;
            TaskDescription.Text = string.Empty;
            AddNewTask.Visibility = Visibility.Collapsed;
            PersonsList.Visibility = Visibility.Collapsed;
            TaskDescription.Visibility = Visibility.Collapsed;
            TaskName.Visibility = Visibility.Collapsed;
            SaveTask.Visibility = Visibility.Collapsed;
            MenuBox.Visibility = Visibility.Visible;

        }
        //Save created person
        private void SavePerson_CLick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var firstName = InsertFirstPersonName.Text;
            var lastName = InsertLastPersonName.Text;
            if (!(string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName)))
            {
                _taskViewModel.AddNewPerson(firstName, lastName);
            }
            InsertFirstPersonName.Text = string.Empty;
            InsertLastPersonName.Text= string.Empty;
            AddNewPerson.Visibility = Visibility.Collapsed;
            InsertFirstPersonName.Visibility = Visibility.Collapsed;
            InsertLastPersonName.Visibility = Visibility.Collapsed;
            SavePerson.Visibility = Visibility.Collapsed;
            AddPeronBlock.Visibility = Visibility.Collapsed;
            MenuBox.Visibility = Visibility.Visible;

        }
        
        //Update task description in DB
        private void UpdateButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var toDoTask = (ToDoTaskModel)TasksList.SelectedItem;
            var description = TextBlock_Description.Text;
            if (!string.IsNullOrEmpty(description))
            {
                _taskViewModel.UpdateTaskInDB(toDoTask, description);
            }
        }
       



    }
}
