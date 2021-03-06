using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
namespace FileAnalyzer
{
    internal class FileDataDao
    {
        public static string database = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\DataBase.json"; //БД
        #region Запись в БД
        public static void WriteDb(List<FileData> fileData)
        {
            var db = new List<string>();
            foreach (var file in fileData)
            {
                //var json = JsonSerializer.Serialize(file);
                var json = JsonConvert.SerializeObject(file);
                db.Add(json);
            }
            File.WriteAllLines(database, db);
        }
        #endregion
        #region Поиск в БД
        public static List<FileData> SearchInDb(string path)
        {
            var db = File.ReadAllLines(database);
            var fileData = new List<FileData>();
            foreach (var json in db)
            {
                //var file = JsonSerializer.Deserialize<FileData>(json);
                var file = JsonConvert.DeserializeObject<FileData>(json);
                file.FullName = file.FullName.Replace(file.Name, "");
                if (file.FullName == (path + "\\"))
                    fileData.Add(file);
            }

            return fileData;
        }
        #endregion
    }
}
