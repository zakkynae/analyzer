using System.Reflection;
using Newtonsoft.Json;
//using System.Text.Json;

namespace FileAnalyzer
{
    public class Program
    {
        static void Main()
        {
            int action;
            do
            {
                PrintMenu();
                Console.Write("Выберите действие: ");
                action = int.Parse(Console.ReadLine());
                if((Menu)action == Menu.GetNewData)
                {
                    Console.Write("Введите директорию, которую хотите отсканировать: ");
                    var path = Console.ReadLine();
                    WriteDb(GetFiles(path));
                }
                if((Menu)action == Menu.GetData)
                {
                    Console.Write("Введите директорию, информацию о которой хотите получить: ");
                    var path = Console.ReadLine();
                    var tuple = SearchInDb(path);
                    if(tuple.count > 0) PrintData(tuple.fileData);
                    else PrintData(GetFiles(path));
                }
            }
            while ((Menu)action != Menu.Quit);
        }

        public static string database = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\DataBase.json"; //БД
        #region Напечатать меню
        public static void PrintMenu()
        {
            foreach (var action in Enum.GetValues(typeof(Menu)))
                Console.WriteLine($"{(int)action}.{action}");
        }
        #endregion
        #region Список данных файлов
        public static List<FileData> GetFiles(string path)
        {
            var fileData = new List<FileData>();
            var dirs = new Stack<string>();
            if(!Directory.Exists(path))
            {
                throw new ArgumentException("Такой дирректории не существует");
            } 
            dirs.Push(path);
            while(dirs.Count > 0)
            {
                var currentDir = dirs.Pop();
                string[] subdirs;
                try
                {
                    subdirs = Directory.GetDirectories(currentDir);
                } catch
                {
                    continue;
                }
                foreach(var dir in subdirs)
                {
                    dirs.Push(dir);
                }
                string[] files;
                try
                {
                    files = Directory.GetFiles(currentDir);
                } catch
                {
                    continue;
                }
                foreach(var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var data = new FileData(fileInfo.FullName, fileInfo.Name, fileInfo.Extension, fileInfo.Length, fileInfo.CreationTime);
                    fileData.Add(data);
                }
            }
            return fileData;
        }
        #endregion
        #region Запись в БД
        public static void WriteDb(List<FileData> fileData)
        {
            var db= new List<string>();
            foreach(var file in fileData)
            {
                //var json = JsonSerializer.Serialize(file);
                var json = JsonConvert.SerializeObject(file);
                db.Add(json);
            }
            File.WriteAllLines(database, db);
        }
        #endregion
        #region Поиск в БД
        public static (List<FileData> fileData, int count)  SearchInDb(string path)
        {
            var db = File.ReadAllLines(database);
            var fileData = new List<FileData>();
            foreach(var json in db)
            {
                //var file = JsonSerializer.Deserialize<FileData>(json);
                var file = JsonConvert.DeserializeObject<FileData>(json);
                if (file.FullName.Contains(path))
                    fileData.Add(file);
            }
            var result = (list: fileData, count: fileData.Count);
            return result;
        }
        #endregion
        #region Вывод данных в консоль
        public static void PrintData(List<FileData> fileData)
        {
            foreach(var file in fileData)
                Console.WriteLine(file.GetString());
        }
        #endregion
    }
}