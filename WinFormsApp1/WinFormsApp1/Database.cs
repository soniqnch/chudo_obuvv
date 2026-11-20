using System;
using MySqlConnector;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class Database
    {
        public static MySqlConnection 
            CreateConnection() { string? connectionString = Environment.GetEnvironmentVariable("CHUDO_OBUV_MYSQL"); 
            return new MySqlConnection(connectionString ?? string.Empty); }
    }

}

