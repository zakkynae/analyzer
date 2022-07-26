using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalyzer
{
    internal class FileDataProvider
    {
        #region Список данных файлов
        public static List<FileData> GetFiles(string path)
        {
            var fileData = new List<FileData>();
            var dirs = new Stack<string>();
            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Такой дирректории не существует");
            }
            dirs.Push(path);
            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();
                string[] subdirs;
                try
                {
                    subdirs = Directory.GetDirectories(currentDir);
                }
                catch
                {
                    continue;
                }
                foreach (var dir in subdirs)
                {
                    dirs.Push(dir);
                }
                string[] files;
                try
                {
                    files = Directory.GetFiles(currentDir);
                }
                catch
                {
                    continue;
                }
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var data = new FileData(fileInfo.FullName, fileInfo.Name, fileInfo.Extension, fileInfo.Length, fileInfo.CreationTime);
                    fileData.Add(data);
                }
            }
            return fileData;
        }
        #endregion
    }
}
