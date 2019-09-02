using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace ThreadTask
{
    public class DataContext : DbContext
    {
        
        public DataContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<HashInfo> HashInfoes { get; set; }
        public DbSet<LogInfo> LogInfoes { get; set; }
    }
    public class HashInfo
    {
        public int Id { get; set; }
        public string NameFile { get; set; }
        public int HashSum { get; set; }
    }
    public class LogInfo
    {
        public int Id { get; set; }
        public string NameFile { get; set; }
        public string ErMes { get; set; }

        public string StTrace { get; set; }
}
}
