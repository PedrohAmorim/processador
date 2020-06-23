using Processador.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Processador
{
    class Program
    {

        static System.Timers.Timer timer;

        public static List<Modulo> modulos = new List<Modulo>();

        public static List<string> mensagens = new List<string>()
        {
            "ST300ALV",
            "ST300CMD"
        };


        static void Main(string[] args)
        {
            carregarModulos();
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;


            // replace the IP with your system IP Address...
            Servidor myserver = new Servidor(IPAddress.Any, 17);

        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            carregarModulos();
            Console.WriteLine("Modulos carregados");
        }

        public static void carregarModulos()
        {
            modulos.Clear();

            using (SqlConnection conSistema = Conexao.GetConnection("Sistema"))
            {
                if (conSistema.State == System.Data.ConnectionState.Closed)
                    conSistema.Open();
                SqlCommand cmd = new SqlCommand(@"select idModulo,idVeiculo,idEmpresa,banco from modulo m join empresa e on e.id = m.idEmpresa  ", conSistema);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    modulos.Add(new Modulo(dr.GetString(0), dr.GetInt32(1), dr.GetInt32(2), dr.GetString(3)));
                }
            }

        }
    }
}
