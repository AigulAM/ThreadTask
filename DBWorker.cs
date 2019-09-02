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
    public class DBWorker
    {
        private readonly CustomQueue<CustomStorage> _hashQueue;
        public DBWorker(CustomQueue<CustomStorage> queue)
        {
            _hashQueue = queue;

        }
        public void Start()
        {
            var thread = new Thread(PrepAdd);
            thread.Start();
        }

        private void PrepAdd()
        {
            if (_hashQueue != null)
            {
                AddHashInDB();
                Console.WriteLine("3 thread complete");
                Log.AutoResetEvent.Set();
            }
            else
            {
                var log = new CustomStorage("", "HashQueue is null");
                Log.BuildLogMessage(log);

            }

        }

        private void AddHashInDB()
        {
            try
            {
                using (var db = new DataContext(Log.connection))
                {
                    while (_hashQueue.Status != StatusWork.Complite)
                    {
                        while (_hashQueue.TryDequeue(out var resultFile))
                        {
                            var dbRow = new HashInfo
                            {
                                NameFile = resultFile.FileName,
                                HashSum = resultFile.HashSum
                            };
                            db.HashInfoes.Add(dbRow);
                            db.SaveChanges();
                        }
                    }
                    _hashQueue.AutoResetEvent.WaitOne();


                }
            }
            catch (Exception e)
            {
                var log = new CustomStorage("", e.Message, e.StackTrace);
                Log.BuildLogMessage(log);
            }
            
        }

        

    }
}
