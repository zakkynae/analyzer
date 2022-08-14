using System.Reflection;
using Newtonsoft.Json;
namespace FileAnalyzer
{
    internal class FileDataDao
    {
        public static string database = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\DataBase.json";

        #region Запись в БД
        public static void WriteDb(List<FileData> fileData)
        {
            if (!File.Exists(database)) File.Create(database);
            var db = new List<string>();
            foreach (var file in fileData)
            {
                var json = JsonConvert.SerializeObject(file);
                db.Add(json);
            }
            File.WriteAllLines(database, db);
        }
        #endregion
        #region Поиск в БД
        public static List<FileData> SearchInDb(string path)
        {
            if (!File.Exists(database)) File.Create(database);
            var db = File.ReadAllLines(database);
            var fileData = new List<FileData>();
            foreach (var json in db)
            {
                var file = JsonConvert.DeserializeObject<FileData>(json);
                file.FullName = file.FullName.Replace(file.Name, "");
                if (file.FullName.Contains(path + "\\"))
                    fileData.Add(file);
            }
            return fileData;
        }
        public static object? GetDataFromBase()
        {
            try
            {
                var db = File.ReadAllLines(database);
                if (string.IsNullOrEmpty(db[0])) return null;
                var file = JsonConvert.DeserializeObject<FileData>(db[0]);
                return file.FullName.Replace($"\\{file.Name}", "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion
    }
}
