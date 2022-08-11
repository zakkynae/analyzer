using System.Reflection;
using Newtonsoft.Json;

namespace FileAnalyzer
{
    public class Program
    {
        static void Main()
        {
            MenuActions action;
            if (FileDataDao.GetDataFromBase() is object) FileDataProvider.Autoscanning();
            while (true)
            {

                Console.Clear();

                FileDataService.PrintMenu();
                Console.Write("Выберите действие: ");
                action = (MenuActions)int.Parse(Console.ReadLine());
                if (action == MenuActions.Quit) return;
                Console.Write("Введите директорию: ");
                var path = Console.ReadLine();
                var files = action == MenuActions.GetNewData || FileDataDao.SearchInDb(path).Count == 0 ? FileDataProvider.GetFiles(path) : FileDataDao.SearchInDb(path);
                switch (action)
                {
                    case MenuActions.GetNewData:
                        FileDataDao.WriteDb(files);
                        Console.WriteLine("База данных обновлена.");
                        break;
                    case MenuActions.GetData:
                        FileDataService.PrintData(files);
                        break;
                    case MenuActions.GetLength:
                        Console.WriteLine("Топ-10 самых больших файлов директории");
                        FileDataService.PrintData(FileDataService.GetLength(files));
                        break;
                    case MenuActions.CopiesLargeFiles:
                        Console.WriteLine("Проверка файлов на наличие копий.");
                        var copies = FileDataService.CopiesLargeFiles(files);
                        if (copies.Count > 0)
                        {
                            Console.WriteLine("Найдены копии следущих файлов: ");
                            FileDataService.PrintData(copies);
                        }
                        else Console.WriteLine("Копий нет.");
                        break;
                    case MenuActions.GetBiggestDirs:
                        Console.WriteLine("Топ-10 самых больших директорий");
                        FileDataService.PrintData(FileDataService.GetBiggestDirs(files));
                        break;
                    case MenuActions.GetExtensions:
                        Console.WriteLine("Топ-10 самых популярных расширений директории");
                        FileDataService.PrintData(FileDataService.GetExtension(files));
                        break;
                    case MenuActions.GetBiggestExtensions:
                        Console.WriteLine("Топ-10 самых больших расширений директории");
                        FileDataService.PrintData(FileDataService.GetBiggestExtensions(files));
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
                        Console.WriteLine("Отредактированные файлы: ");
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
