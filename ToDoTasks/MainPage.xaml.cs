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
        
        public List<MainMenuPointsModel> MenuStatsPoints { get; private set; } = new List<MainMenuPointsModel>()
        { 
            new MainMenuPointsModel(MenuStats.Add_new_Task, "Add new Task"), 
            new MainMenuPointsModel(MenuStats.Add_new_Person, "Add new Person")
        };
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
        
        /// <summary>
        /// Shows info about the selected task from ListView collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Checking the receiving data from the [ViewModel] and get the relevant script for showing MainMenuBox
        /// If the data doesn't contain the saved ToDoTask and the Persons it shows only the [Add new Person] menu point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentMenuPoint = (MainMenuPointsModel)MenuBox.SelectedItem;
            if (_taskViewModel.IsDataExist())
            {
                switch (currentMenuPoint.MenuStats)
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
                switch (currentMenuPoint.MenuStats)
                {
                    case MenuStats.Add_new_Person:
                        MenuBoxAddPerson_SelectionChanged(sender, e);
                        break;
                }
            }
        }
        /// <summary>
        /// Shows only the [Add new Task] form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuBoxAddTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddNewTask.Visibility = Visibility.Visible;
            PersonsList.Visibility = Visibility.Visible;
            CloseAllMenus.Visibility = Visibility.Visible;
            MenuBox.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Shows only the [Add new Person] form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuBoxAddPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddNewPerson.Visibility = Visibility.Visible;
            InsertFirstPersonName.Visibility = Visibility.Visible;
            InsertLastPersonName.Visibility = Visibility.Visible;
            SavePerson.Visibility = Visibility.Visible;
            AddPeronBlock.Visibility = Visibility.Visible;
            CloseAllMenus.Visibility = Visibility.Visible;
            MenuBox.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Select the actual person for create a new ToDoTask
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Persons_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {            
            TaskDescription.Visibility = Visibility.Visible;
            TaskName.Visibility = Visibility.Visible;
            SaveTask.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// The save button for create a new ToDoTask and  sends the new Task data into [ViewModel]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            CloseAllMenus.Visibility = Visibility.Collapsed;
            MenuBox.Visibility = Visibility.Visible;

        }
        /// <summary>
        /// The save button for create a new Person and sends the new Task data into [ViewModel]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            CloseAllMenus.Visibility = Visibility.Collapsed;
            MenuBox.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// The update button for create a new text description and sends it into [ViewModel]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var toDoTask = (ToDoTaskModel)TasksList.SelectedItem;
            var description = TextBlock_Description.Text;
            if (!string.IsNullOrEmpty(description))
            {
                _taskViewModel.UpdateTaskInDB(toDoTask, description);
            }
        }
        /// <summary>
        /// The close button for close all forms and shows the MainMenu ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CLoseMainMenu_CLick(object sender, RoutedEventArgs e)
        {
            AddNewTask.Visibility = Visibility.Collapsed;
            PersonsList.Visibility = Visibility.Collapsed;
            TaskDescription.Visibility = Visibility.Collapsed;
            TaskName.Visibility = Visibility.Collapsed;
            SaveTask.Visibility = Visibility.Collapsed;
            AddNewPerson.Visibility = Visibility.Collapsed;
            InsertFirstPersonName.Visibility = Visibility.Collapsed;
            InsertLastPersonName.Visibility = Visibility.Collapsed;
            SavePerson.Visibility = Visibility.Collapsed;
            AddPeronBlock.Visibility = Visibility.Collapsed;
            CloseAllMenus.Visibility = Visibility.Collapsed;
            MenuBox.Visibility = Visibility.Visible;
        }
       



    }
}
