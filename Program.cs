using System;



namespace ThreadTask
{
    public class Program
    {

        public static void Main(string[] args)
        {
            bool repeat = true;
            while (repeat)
            {
                string provider = @"System.Data.SqlClient";
                string severName = @"AKHMETOVA-AM";
                string dirDef = @"C:\Users\All Users\Intel\";
                Console.WriteLine(@"Введите директорию");
                var dir = Console.ReadLine();
                if (dir == String.Empty) dir = dirDef;
                var scanner = new FileScannerWorker();
                var dirValid = scanner.CheckDir(dir);
                if (!dirValid) continue;
                Console.WriteLine(@"Введите имя сервера");
                if (Console.ReadLine() != String.Empty) severName = Console.ReadLine();
                Log.connection = $"data source={severName};Initial Catalog=Thread;Integrated Security=True;";
                scanner.Start(dir);
                var calc = new CalcHashWorker(scanner.FileQueue);
                calc.Start();
                var dbWorker = new DBWorker(calc.HashQueue);
                dbWorker.Start();
                Log.AutoResetEvent.WaitOne();
                Log.WriteLog();
                Console.WriteLine("Main thread complete");
                Console.WriteLine("Закрыть окно? 0-закрыть, 1-ввести новый каталог");
                switch (Console.ReadLine())
                {
                    case "1": repeat = true;
                        break;
                    default: repeat = false;
                        break;
                }
            }
        }

    }
}
