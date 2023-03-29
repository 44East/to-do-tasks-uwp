using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Foundation;
using System.Linq;
using System.Text.RegularExpressions;
using ToDoTasks.ViewModel;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.ApplicationModel.Core;
using Windows.UI;
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


        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddTask.IsSelected)
            {
                Frame.Navigate(typeof(AddTaskPage), _taskViewModel);
            }
            else if (AddPerson.IsSelected)
            {
                Frame.Navigate(typeof(AddPersonPage), _taskViewModel);
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
        /// Method for the controlling of the user ipnut it prohibits a special symbols
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TextBox_OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            char[] chars = { '-', '.', ' ', '!', '?', ':', ';' };
            sender.Text = new String(sender.Text.Where(c => (char.IsLetterOrDigit(c) || chars.Contains(c))).ToArray());
        }




    }
}
