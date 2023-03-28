using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using ToDoTasks.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
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
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddTask.IsSelected)
            {
                Frame.Navigate(typeof(AddTaskPage), _taskViewModel);
            }
            else if (AddPerson.IsSelected)
            {
                Frame.Navigate(typeof(AddPersonPage));
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        /// <summary>
        /// Shows info about the selected task from ListView collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToDoTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToDoTaskModel taskModel = (ToDoTaskModel)TasksList.SelectedItem;
            _taskViewModel.ToDoTask = taskModel;
            if (!Frame.CanGoBack) //After the task collection is refreshed, the ListView collection is also refreshed,
                                  //and because the task instance is selected, a cyclic transition to the task editing page is made.                 
                Frame.Navigate(typeof(TaskWindow), _taskViewModel, new SlideNavigationTransitionInfo());
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
        /// <summary>
        /// Method for the controlling of the user ipnut it prohibits a special symbols
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TextBox_OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            char[] chars = { '-', '.', ' ', '!', '?', ':', ';'};
            sender.Text = new String(sender.Text.Where(c => (char.IsLetterOrDigit(c) || chars.Contains(c))).ToArray());
        }




    }
}
