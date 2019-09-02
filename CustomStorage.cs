using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTask
{
    public class CustomStorage
    {
        public string FileName;
        public int HashSum;
        public string ErMes;
        public string StTrace;
        public CustomStorage(string fileName, int hashSum)
        {
            FileName = fileName;
            HashSum = hashSum;
        }

        public CustomStorage(string fileName, string errorMessage, string stackTrace)
        {
            FileName = fileName;
            ErMes = errorMessage;
            StTrace = stackTrace;
        }
        public CustomStorage(string fileName, string errorMessage)
        {
            FileName = fileName;
            ErMes= errorMessage;
        }
    }
}
