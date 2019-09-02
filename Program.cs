using System;



namespace ThreadTask
{
    public class Program
    {

        public static void Main(string[] args)
        {
            for (int i = 0; i < 1; i++)
            {
                string provider = @"System.Data.SqlClient";
                string severName = @"AKHMETOVA-AM";
                string dirDef = @"C:\Users\All Users\Intel\";
                Console.WriteLine(@"Введите директорию");
                var dir = Console.ReadLine();
                if (dir != String.Empty) dir= dirDef ;
                Console.WriteLine(@"Введите имя сервера");
                if (Console.ReadLine() != String.Empty) severName = Console.ReadLine();
                Log.connection = $"data source={severName};Initial Catalog=Thread;Integrated Security=True;";
                var scanner = new FileScannerWorker();
                var dirValid = scanner.CheckDir(dir);
                var calc = new CalcHashWorker(scanner.FileQueue);
                calc.Start();
                var dbWorker = new DBWorker(calc.HashQueue);
                dbWorker.Start();
                Log.AutoResetEvent.WaitOne();
                Log.WriteLog();
                Console.WriteLine("Main thread complete");
            }

            Console.ReadLine();
        }

    }
}
