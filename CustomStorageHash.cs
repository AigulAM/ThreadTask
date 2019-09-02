using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTask
{
    public class CustomStoregeHash
    {
        public byte[] Hash { get;  }

        public string FileInfo { get; }

        public CustomStoregeHash(string fileName, byte[] hash)
        {
            FileInfo = fileName;
            Hash = hash;
        }
    }
}
