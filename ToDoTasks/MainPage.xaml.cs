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
        private TasksViewModel _taskViewModel;
        public ObservableCollection<ToDoTaskModel> ToDoTasks { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            this._taskViewModel = new TasksViewModel();
            this.DataContext = this._taskViewModel;
            ToDoTasks = this._taskViewModel.ToDoTasks;
            TasksList.ItemsSource = this.ToDoTasks;
            ApplicationView.GetForCurrentView().Title = "Main Menu";


        }
        /// <summary>
        /// The side panell menu selector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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



    }
}
