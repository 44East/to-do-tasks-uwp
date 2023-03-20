using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ToDoTasks.Model.Connection
{ 
    internal class FileWorker<T>
    {
        public T GetModelFromFile(string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            using (StreamReader sr = file.OpenText())
            {
                var prepareString = sr.ReadToEnd();
                return JsonSerializer.Deserialize<T>(prepareString);
            }
        }
        public void InsertModelInFile(T item, string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                Utf8JsonWriter jsonWriter = new Utf8JsonWriter(fs);
                JsonSerializer.Serialize<T>(jsonWriter, item);
            }
        }

    }
}
