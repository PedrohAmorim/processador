using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Processador
{

    class Servidor
    {
        TcpListener server = null;
        public Servidor(IPAddress ip, int port)
        {
       
            server = new TcpListener(ip, port);
            server.Start();
            StartListener();
        }
        public void StartListener()
        {
            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Thread t = new Thread(new ParameterizedThreadStart(processarMensagem));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }
        public void processarMensagem(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            string imei = String.Empty;
            string Data = null;
            Byte[] bytes = new Byte[256];
            int i;
            try
            {
                
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                   
                        string hex = BitConverter.ToString(bytes);
                        Data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Mensagem: {0}", Data);

                       string[] mensagens = Data.Split(' ');

                    foreach (var msg in mensagens)
                    {

                        //Criar array de dados
                        var dados = msg.Split(';');

                        if (!Program.mensagens.Contains(dados[0]) && dados.Count() > 15)
                        {

                            //Formatar data no formato correto
                            char[] data = dados[4].ToCharArray();
                            DateTime dataehora = DateTime.Parse(string.Concat(data[6], data[7], '/', data[4], data[5], '/', data[0], data[1], data[2], data[3], ' ', dados[5]));

                            //Verificar Ignição
                            char[] ignicao = dados[15].ToCharArray();

                            var modulo = Program.modulos.Where(x => x.id == dados[1]).FirstOrDefault();

                            using (SqlConnection con = Conexao.GetConnection(modulo.banco))
                            {
                                if (con.State == System.Data.ConnectionState.Closed)
                                    con.Open();

                                SqlCommand cmd = new SqlCommand("insert into posicao(idModulo,idVeiculo,dataehora,mensagem,latitude,longitude,Odometro,velocidade,ignicao) values(@idmodulo,@idVeiculo,@dataehora,@msg,@latitude,@longitude,@odometro,@velocidade,@ignicao)", con);
                                cmd.Parameters.Add(new SqlParameter("@ignicao", ignicao[0]));
                                cmd.Parameters.Add(new SqlParameter("@velocidade", dados[9]));
                                cmd.Parameters.Add(new SqlParameter("@odometro", dados[13]));
                                cmd.Parameters.Add(new SqlParameter("@latitude", dados[7]));
                                cmd.Parameters.Add(new SqlParameter("@longitude", dados[8]));
                                cmd.Parameters.Add(new SqlParameter("@idveiculo", modulo.idVeiculo));
                                cmd.Parameters.Add(new SqlParameter("@idmodulo", modulo.id));
                                cmd.Parameters.Add(new SqlParameter("@dataehora", dataehora));
                                cmd.Parameters.Add(new SqlParameter("@msg", Data));
                                cmd.ExecuteNonQuery();
                            }
                            /*string str = "Hey Device!";
                            Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);
                            stream.Write(reply, 0, reply.Length);
                            Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
                            */
                        }
                        else
                        {

                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                client.Close();
            }
            
        }
    }
}