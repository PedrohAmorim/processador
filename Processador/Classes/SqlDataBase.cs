using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Processador
{
    public class DataBase
    {

        public static SqlConnection connectSql(string server, string database, string user, string password, bool open)
        {
            var conn = new SqlConnection(string.Format(ConfigurationManager.AppSettings.Get("SQLCONNECTION"), server, database, user, password));

            if (open) { conn.Open(); }

            return conn;
        }

        public static SqlConnection connectPlugAndPlay(bool open)
        {
            var conn = new SqlConnection(ConfigurationManager.AppSettings.Get("PLUGANDPLAY"));

            if (open) { conn.Open(); }

            return conn;
        }


    }
}
