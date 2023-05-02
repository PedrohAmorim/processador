using Processador.Classes;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Processador
{
    public enum TypeMessage
    {
        EVENT = 0,
        TRACK = 1
    }
    class Server
    {
        TcpListener server = null;
        public Server(IPAddress ip, int port)
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
                    Thread t = new Thread(new ParameterizedThreadStart(ProcessMessage));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }


        public void ProcessMessage(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            Byte[] bytes = new Byte[256];
            int i = 0;

            try
            {
                //do
                //{
                //    var streamByte = BitConverter.ToString(stream.ReadByte()).Split('-'); ;
                //} while (stream.ReadByte() != -1);

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {

                    string[] hex = BitConverter.ToString(bytes).Split('-');
                    //string data = Encoding.ASCII.GetString(bytes, 0, i);

                    var header = new PacketHeader(hex);

                    Console.WriteLine("Mensagem: {0}", Misc.arrayToString(hex, 0, header.MessageSize, false));


                    if (header.MessageType == (int)TypeMessage.EVENT)
                    {

                    }
                    else if (header.MessageType == (int)TypeMessage.TRACK)
                    {
                        Track newTrack = new Track(hex, header);
                        Console.Write(newTrack.ToString());
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