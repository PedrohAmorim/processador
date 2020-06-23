using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador
{
    public class Conexao
    {

        public static SqlConnection GetConnection(string banco)
        {
            return  new SqlConnection(string.Format("Data Source=142.44.203.34;Initial Catalog={0};Persist Security Info=True;User ID=processador;Password=pP@191915", banco));
        }
    }
}
    