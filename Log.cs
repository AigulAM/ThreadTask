using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTask
{
    public class Log
    {

        public static Queue<CustomStorage> LogQueue = new Queue<CustomStorage>();
        public static AutoResetEvent AutoResetEvent = new AutoResetEvent(false);
        public static string connection;
        public static void WriteLog()
        {
            if (LogQueue != null)
                while (LogQueue.Count != 0)
                    using (var db = new DataContext(Log.connection))
                    {
                        var log = LogQueue.Dequeue();
                        var logRow = new LogInfo
                        {
                            ErMes = log.ErMes,
                            StTrace = log.StTrace,
                            NameFile = log.FileName
                        };
                        db.LogInfoes.Add(logRow);
                        db.SaveChanges();
                    }
        }

        public static void BuildLogMessage(CustomStorage log)
        {
            Log.LogQueue.Enqueue(log);
            Log.AutoResetEvent.Set();
        }

    }
}
