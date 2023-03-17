using System.Text.Json;

namespace ConnectorToSQL
{
    internal class FileWorker<T>
    {
        public T GetModelFromFile(string fileName)
        {
            FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + fileName);
            using StreamReader sr = file.OpenText();
            var prepareString = sr.ReadToEnd();

            return JsonSerializer.Deserialize<T>(prepareString);
        }
        public void InsertModelInFile(T item, string filename)
        {
            using var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + filename, FileMode.Create);
            JsonSerializer.Serialize(fs, item);
        }

    }
}
