
namespace ToDoTasks
{
    /// <summary>
    /// The model class for the application [View] it contains the model of [MainMenuBox] in the [View] part app
    /// </summary>
    public enum MenuStats
    {
        Add_new_Task,
        Add_new_Person
    }
    public class MainMenuPointsModel
    {
        public MainMenuPointsModel(MenuStats menuStats, string menuPoints)
        {

            MenuStats = menuStats;
            MenuPointName = menuPoints;

        }
        public string MenuPointName { get; set; }
        public MenuStats MenuStats { get; set; }
    }
}
