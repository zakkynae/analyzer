using System.Reflection;
using Newtonsoft.Json;
//using System.Text.Json;

namespace FileAnalyzer
{
    public class Program
    {
        static void Main()
        {
            MenuActions action;

            while (true)
            {
                Console.Clear();
                FileDataService.PrintMenu();
                Console.Write("Выберите действие: ");
                action = (MenuActions)int.Parse(Console.ReadLine());
                if (action == MenuActions.Quit) return;
                Console.Write("Введите директорию: ");
                var path = Console.ReadLine();
                var files = FileDataDao.SearchInDb(path);
                switch (action)
                {
                    case MenuActions.GetNewData:
                        FileDataDao.WriteDb(FileDataProvider.GetFiles(path));
                        Console.WriteLine("База данных обновлена.");
                        break;
                    case MenuActions.GetData:
                        if (files.Count > 0) FileDataService.PrintData(files);
                        else FileDataService.PrintData(FileDataProvider.GetFiles(path));
                        break;
                    case MenuActions.GetLength:
                        Console.WriteLine("Топ-10 самых больших файлов директории");
                        if (files.Count > 0) FileDataService.PrintData(FileDataService.GetLength(files));
                        else FileDataService.PrintData(FileDataService.GetLength(FileDataProvider.GetFiles(path)));
                        break;
                    case MenuActions.GetBiggestDirs:
                        Console.WriteLine("Топ-10 самых больших директорий");
                        if (files.Count > 0) FileDataService.PrintData(FileDataService.GetBiggestDirs(files));
                        else FileDataService.PrintData(FileDataService.GetBiggestDirs(FileDataProvider.GetFiles(path)));
                        break;
                    case MenuActions.GetExtensions:
                        Console.WriteLine("Топ-10 самых популярных расширений директории");
                        if (files.Count > 0) FileDataService.PrintData(FileDataService.GetExtension(files));
                        else FileDataService.PrintData(FileDataService.GetExtension(FileDataProvider.GetFiles(path)));
                        break;
                    case MenuActions.GetBiggestExtensions:
                        Console.WriteLine("Топ-10 самых больших расширений директории");
                        if (files.Count > 0) FileDataService.PrintData(FileDataService.GetBiggestExtensions(files));
                        else FileDataService.PrintData(FileDataService.GetBiggestExtensions(FileDataProvider.GetFiles(path)));
                        break;
                    case MenuActions.GetChangesFiles:
                        var infoDirectory = FileDataService.GetChangesFiles(path);
                        Console.WriteLine("Новые файлы: ");
                        if (infoDirectory.newFiles.Count == 0) Console.WriteLine("Новых файлов нет.");
                        else FileDataService.PrintData(infoDirectory.newFiles);
                        Console.WriteLine("Удаленные файлы: ");
                        if (infoDirectory.deletedFiles.Count == 0) Console.WriteLine("Удаленных файлов нет.");
                        else FileDataService.PrintData(infoDirectory.deletedFiles);
                        Console.WriteLine("Пересозданные файлы: ");
                        if (infoDirectory.newTimeCreation.Count == 0) Console.WriteLine("Пересозданных файлов нет.");
                        else FileDataService.PrintData(infoDirectory.newTimeCreation);
                        Console.WriteLine("Отредоктированные файлы: ");
                        if (infoDirectory.newLength.Count == 0) Console.WriteLine("Отредактированных файлов нет.");
                        else FileDataService.PrintData(infoDirectory.newLength);
                        break;
                }
                Console.WriteLine("Для продолжения нажмите любуюу клавишу...");
                Console.ReadKey();
            }
        }
    }
}