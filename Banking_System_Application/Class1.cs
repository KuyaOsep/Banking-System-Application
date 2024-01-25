using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Banking_System_Application
{
    class dbConnection
    {
        public string dbConnect()
        {
            string connect = "server=localhost; user=root; password=; database=bank";
            return connect;
        }
    }
}
