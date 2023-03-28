using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ToDoTasks.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ToDoTasks
{
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskWindow : Page
    {
        private TasksViewModel _tasksViewModel;
        private ToDoTaskModel _toDoTaskModel;
        public TaskWindow()
        {
            this.InitializeComponent();
                       
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _tasksViewModel = (TasksViewModel)e.Parameter;
            _toDoTaskModel = _tasksViewModel.ToDoTask;
            this.DataContext = _tasksViewModel;

            TextBlock_FName.Text = _toDoTaskModel.PersonFirstName + " " + _toDoTaskModel.PersonLastName;
            TextBox_Description.Text = _toDoTaskModel.Description;
        }
        /// <summary>
        /// The update button for create a new text description and sends it into [ViewModel]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var description = TextBox_Description.Text;
            if (!string.IsNullOrEmpty(description))
            {
                _tasksViewModel.UpdateTaskInDB(_toDoTaskModel, description);
            }
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
        /// <summary>
        /// Deleting button for the current task
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_toDoTaskModel != null)
                _tasksViewModel.DeleteTask(_toDoTaskModel.ID);
            if (Frame.CanGoBack)
                Frame.GoBack();
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
