using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTasks
{
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
