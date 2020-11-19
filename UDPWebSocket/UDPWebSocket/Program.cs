using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

            try
            {
                _listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Task listeningTask = new Task(Listen);
                listeningTask.Start();

                while (true)
                {
                    string message = Console.ReadLine();

                    byte[] data = Encoding.Unicode.GetBytes(message);
                    EndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _remotePort);
                    _listeningSocket.SendTo(data, remotePoint);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        private static void Listen()
        {
            try
            {
                EndPoint localIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _localPort);
                _listeningSocket.Bind(localIp);

                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] buffer = new byte[256];
                
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                    do
                    {
                        bytes = _listeningSocket.ReceiveFrom(buffer, ref remoteIp);
                        builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                    } while (_listeningSocket.Available > 0);
                    
                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint;
                    
                    Console.WriteLine("{0}:{1} - {2}", remoteFullIp.Address, remoteFullIp.Port, builder);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        private static void Close()
        {
            if (_listeningSocket != null)
            {
                _listeningSocket.Shutdown(SocketShutdown.Both);
                _listeningSocket.Close();
                _listeningSocket = null;
            }
        }
    }
}