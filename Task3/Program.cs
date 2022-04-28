using System;
using System.IO;
namespace FileSystem
{
    class Program
    {
        
        static DateTime datetimeend = DateTime.Now;
        static void Main(string[] args)
        {
            long SizeFilesBegin = 0;
            long SizeFilesEnd = 0;
            int FileBegin = 0;
            int FileEnd =0;

            string rootDir;
            //задаем папку 
            if (args.Length == 0)
                rootDir = @"e:\garmin\";
            else
                rootDir = args[0].ToString();


            //вызываем рекурсивный метод
            Walk(rootDir, ref SizeFilesBegin, ref FileBegin);
            Console.WriteLine($"Размер папки до удаления {SizeFilesBegin}");
            Console.WriteLine($"Количество файлов до удаления {FileBegin}");
            datetimeend = datetimeend - TimeSpan.FromMinutes(30);
            DeleteFolder(rootDir);
            Walk(rootDir, ref SizeFilesEnd, ref FileEnd);
            Console.WriteLine($"Размер папки после удаления {SizeFilesEnd}");
            Console.WriteLine($"Количество файлов после удаления {FileEnd}");

            Console.WriteLine($"Удалено в байтах {SizeFilesBegin - SizeFilesEnd}");
            Console.WriteLine($"Удалено файлов {FileBegin - FileEnd}");
        }

        static void Walk(string rootDir, ref long SizeFiles, ref int iFile)
        {
            DirectoryInfo root = (new DirectoryInfo(rootDir));
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            // Получаем все файлы в текущем каталоге
            try
            {
                files = root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            if (files != null)
            {
              
                foreach (FileInfo fi in files)
                {
                    //Console.WriteLine(fi.FullName);
                    SizeFiles += fi.Length;
                    iFile++;
                }

                //получаем все подкаталоги
                subDirs = root.GetDirectories();
                //проходим по каждому подкаталогу
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    //РЕКУРСИЯ
                    Walk(dirInfo.Name, ref SizeFiles, ref iFile);
                }
            }
        }



        static void DeleteFolder(string folder)   //
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                DirectoryInfo[] diA = di.GetDirectories();
                FileInfo[] fi = di.GetFiles();
                foreach (FileInfo f in fi)
                {
                    if (f.LastAccessTime < datetimeend)
                        f.Delete();
                }
                foreach (DirectoryInfo df in diA)
                {
                    DeleteFolder(df.FullName);
                }
                if (di.GetDirectories().Length == 0 && di.GetFiles().Length == 0) di.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }
        }
  
    }
}