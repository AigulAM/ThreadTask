using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Security.Cryptography;

namespace ThreadTask
{
    public class FileScannerWorker
    {
        private Thread _thread;
        private AutoResetEvent _autoResetEvent;
        private List<string> ListFileName = new List<string>();
        public CustomQueue<string> FileQueue;
        public FileScannerWorker()
        {
            _autoResetEvent = new AutoResetEvent(true);
            FileQueue = new CustomQueue<string>(_autoResetEvent);
        }

        public bool CheckDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                var log = new CustomStorage(dirPath, "You enter incorrect dir");
                Log.BuildLogMessage(log);
                return false;
            }
            return true;
        }

        public void Start(string dirPath)
        {
            _thread = new Thread(PrepScan);
            _thread.Start(dirPath);
        }
        private void PrepScan(object dirPath)
        {
            FileQueue.Status = StatusWork.Running;
            Scan(dirPath.ToString());
            FileQueue.Status = StatusWork.Complite;
            Console.WriteLine($"count files { ListFileName.Count()}");
            Console.WriteLine(@"1 thread complete");
        }
        private void Scan(string dirPath)
        {
            try
            {
                foreach (var dir in Directory.EnumerateDirectories(dirPath))
                {
                    Scan(dir);
                }
                foreach (var item in Directory.GetFiles(dirPath))
                {
                    ListFileName.Add(item);
                    FileQueue.Enqueue(item);

                }
            }
            catch (Exception e)
            {
                var log = new CustomStorage(dirPath, e.Message,e.StackTrace);
                Log.BuildLogMessage(log);
            }

        }
    }
}
