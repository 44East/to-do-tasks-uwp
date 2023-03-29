using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ToDoTasks.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
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
    public sealed partial class AddPersonPage : Page
    {
        private TasksViewModel _tasksViewModel;
        public AddPersonPage()
        {
            this.InitializeComponent();
            ApplicationView.GetForCurrentView().Title = "Add new Person";
        }
        /// <summary>
        /// Redirection method, it bibndings all fields and property from an incoming parameters
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _tasksViewModel = (TasksViewModel)e.Parameter;
            this.DataContext = _tasksViewModel;
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
                _tasksViewModel.AddNewPerson(firstName, lastName);
            }
            InsertFirstPersonName.Text = string.Empty;
            InsertLastPersonName.Text = string.Empty;
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
