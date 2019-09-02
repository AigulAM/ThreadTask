using System;
using System.Threading;
using System.IO;
using System.Security.Cryptography;

namespace ThreadTask
{
    class CalcHashWorker
    {
        private CustomQueue<string> _fileinfo;
        private CustomQueue<CustomStorage> _hashQueue;
        private AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
        public CustomQueue<CustomStorage> HashQueue => _hashQueue;
        private Thread _thread;
        public CalcHashWorker(CustomQueue<string> queue)
        {
            _fileinfo = queue;
        }

        public void Start()
        {
            if (_fileinfo != null)
            {
                _hashQueue = new CustomQueue<CustomStorage>(_autoResetEvent);
                _thread = new Thread(GetHash);
                _thread.Start();
            }
            else
            {
                var log = new CustomStorage("", "FileQueue is null");
                Log.BuildLogMessage(log);
            }
        }
        private void GetHash()
        {
            _hashQueue.Status = StatusWork.Running;
            Calc();
            _hashQueue.Status = StatusWork.Complite;
            Console.WriteLine($"count hash { _hashQueue.Count()}");
            Console.WriteLine(@"2 thread complete");
        }
        private void Calc()
        {
            try
            {
                while (_fileinfo.Status!=StatusWork.Complite)
                {
                    while (_fileinfo.TryDequeue(out var resultFile))
                    {
                        using (var md5Hash = MD5.Create())
                        {
                            using (var stream = File.OpenRead(resultFile))
                            {
                                var hash = new CustomStorage(resultFile, BitConverter.ToInt32(md5Hash.ComputeHash(stream), 0));
                                _hashQueue.Enqueue(hash);
                            }
                        }
                    }
                    _fileinfo.AutoResetEvent.WaitOne();
                }
                while (_fileinfo.TryDequeue(out var resultFile))
                {
                    using (var md5Hash = MD5.Create())
                    {
                        using (var stream = File.OpenRead(resultFile))
                        {
                            var hash = new CustomStorage(resultFile, BitConverter.ToInt32(md5Hash.ComputeHash(stream),0));
                            _hashQueue.Enqueue(hash);
                        }

                    }
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
