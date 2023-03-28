using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ToDoTasks.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ToDoTasks
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTaskPage : Page
    {
        private TasksViewModel _tasksViewModel;
        public AddTaskPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _tasksViewModel = (TasksViewModel)e.Parameter;
            this.DataContext = _tasksViewModel;
            this.PersonsList.ItemsSource = _tasksViewModel.Persons;
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
            var description = TaskDescription.Text;
            if (!(string.IsNullOrEmpty(taskName) && string.IsNullOrEmpty(description)))
            {
                _tasksViewModel.AddNewTask(person, taskName, description);
            }
            TaskName.Text = string.Empty;
            TaskDescription.Text = string.Empty;
            if (Frame.CanGoBack)
                Frame.GoBack();

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
