using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTasks.Model.DataOperations
{
    internal class DirectoryCreator
    {
        internal static void CreateDBDirectory()
        {
            Directory.CreateDirectory($@"D:\Application Data\ToDoTasks\db_data\");
        }
    }
}
