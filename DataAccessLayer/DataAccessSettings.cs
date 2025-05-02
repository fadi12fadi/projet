using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class DataAccessSettings
    {
        // connection string with relative path
        static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "dbv7.db");
        public static string connectionString = $"Data Source={dbPath};";
    }
}
