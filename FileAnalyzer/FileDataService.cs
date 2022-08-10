using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalyzer
{
    internal class FileDataService
    {
        #region Напечатать меню
        public static void PrintMenu()
        {
            foreach (var action in Enum.GetValues(typeof(MenuActions)))
                Console.WriteLine($"{(int)action}.{action}");
        }
        #endregion

        #region Топ 10 больших файлов дирректорий
        public static List<FileData> GetLength(List<FileData> files)
        {
            var sortedFiles = files.OrderByDescending(f => f.Length).ToList();
            var lengths = new List<FileData>();
            if(sortedFiles.Count >=10 )
                for (int i = 0; i < 10; i++)
                    lengths.Add(sortedFiles[i]);
            else
            {
                for (int i = 0; i < sortedFiles.Count; i++)
                    lengths.Add(sortedFiles[i]);
            }
            return lengths;
        }
        public static List<FileData> CopiesLargeFiles(List<FileData> files)
        {
            var dictionary = new Dictionary<FileData, int>();
            foreach (var e in files)
            {
                if (!dictionary.ContainsKey(e)) dictionary[e] = 0;
                dictionary[e]++;
            }
            return dictionary
                .Where(x => x.Value > 1)
                .Select(x => x.Key)
                .ToList();
        }
        #endregion
        #region Топ 10 больших директорий
        public static string[] GetBiggestDirs(List<FileData> files)
        {
            var dirs = new Dictionary<string, long>();
            foreach (var file in files)
            {
                file.FullName = file.FullName.Replace(file.Name, "");
                if (!dirs.ContainsKey(file.FullName)) dirs[file.FullName] = 0;
                dirs[file.FullName] = +file.Length;
            }
            dirs = dirs.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            var extensionsDir = dirs.Keys.ToList();
            string[] biggestDirs;
            if (extensionsDir.Count >= 10)
            {
                biggestDirs = new string[10];
                for (int i = 0; i < 10; i++)
                    biggestDirs[i] = extensionsDir[i];
            }
            else
            {
                biggestDirs = new string[extensionsDir.Count];
                for (int i = 0; i < extensionsDir.Count; i++)
                    biggestDirs[i] = extensionsDir[i];
            }
            return biggestDirs;
        }
        #endregion
        #region Топ 10 расширений
        public static string[] GetExtension(List<FileData> files)
        {
            var extension = new Dictionary<string, int>();
            foreach (var file in files)
            {
                if (!extension.ContainsKey(file.Extension)) extension[file.Extension] = 0;
                extension[file.Extension]++;
            }
            extension = extension.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            var extensionsDir = extension.Keys.ToList();
            var firstTenExtensions = new string[10];
            for (int i = 0; i < 10; i++)
                firstTenExtensions[i] = extensionsDir[i];
            return firstTenExtensions;
        }
        #endregion
        #region Топ 10 расширений по суммарному объему
        public static string[] GetBiggestExtensions(List<FileData> files)
        {
            var extension = new Dictionary<string, long>();
            foreach (var file in files)
            {
                if (!extension.ContainsKey(file.Extension)) extension[file.Extension] = 0;
                extension[file.Extension] = +file.Length;
            }
            extension = extension.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            var extensionsDir = extension.Keys.ToList();
            var biggestExtension = new string[10];
            for (int i = 0; i < 10; i++)
                biggestExtension[i] = extensionsDir[i];
            return biggestExtension;
        }
        #endregion
        #region Вывод данных в консоль
        public static void PrintData<T>(IEnumerable<T> fileData)
        {
            foreach (var file in fileData)
                Console.WriteLine(file);
        }
        #endregion
        #region Изменение файлов
        public static (List<FileData> newFiles, List<FileData> deletedFiles, List<FileData> newLength, List<FileData> newTimeCreation) GetChangesFiles(string path)
        {
            var oldDb = FileDataDao.SearchInDb(path);
            if (oldDb.Count == 0)
            {
                throw new Exception("В прежней базе этой директории нет. Изменения не удсатся определить.");
            }
            var newDb = FileDataProvider.GetFiles(path);
            var newFiles = new List<FileData>(newDb);
            var deletedFiles = new List<FileData>();
            var newLength = new List<FileData>();
            var newTimeCreation = new List<FileData>();
            foreach (var file in oldDb)
            {
                newFiles.RemoveAll(x => x.Name == file.Name);
                if (!newDb.Any(x => x.Name == file.Name)) deletedFiles.Add(file);
                if (newDb.Any(x => x.Name == file.Name && x.Length != file.Length)) newLength.Add(file);
                if (newDb.Any(x => x.Name == file.Name && x.CreationDate != file.CreationDate)) newTimeCreation.Add(file);
            }
            var result = (newFiles: newFiles, deletedFiles: deletedFiles, newLength: newLength, newTimeCreation: newTimeCreation);
            return result;
        }
        #endregion
    }
}
