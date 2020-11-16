using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPWebSocket
{
    class Program
    {
        private static int _localPort;
        private static int _remotePort;
        private static Socket _listeningSocket;
        
        static void Main(string[] args)
        {
            Console.Write("Введите порт для приема сообщений: ");
            _localPort = int.Parse(Console.ReadLine());
            
            Console.Write("Введите порт для отправки сообщений: ");
            _remotePort = int.Parse(Console.ReadLine());
            
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");
            Console.WriteLine();
            
            
        }

        private static void Listen()
        {
            try
            {
                EndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _localPort);
                _listeningSocket.Bind(localEndPoint);
                
                StringBuilder builder = new StringBuilder();
                byte[] buffer = new byte[256];
                
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                do
                {
                    int bytes = _listeningSocket.ReceiveFrom(buffer, ref remoteEndPoint);
                    builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                } while (_listeningSocket.Available > 0);

                if (remoteEndPoint is IPEndPoint remoteFullIp)
                    Console.WriteLine("{0}:{1} - {2}", 
                        remoteFullIp.Address,
                        remoteFullIp.Port, builder);
            }
            catch
            {

            }
            finally
            {
                
            }
        }
    }
}