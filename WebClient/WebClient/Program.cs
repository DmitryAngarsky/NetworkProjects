using System;
using System.Net;
using System.Threading.Tasks;

namespace WebClient
{
    class Program
    {
        static async Task Main()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://translate.google.com");
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            WebHeaderCollection headers = response.Headers;
            
            for (int i = 0; i < headers.Count; i++)
            {
                Console.WriteLine("{0}: {1}", headers.GetKey(i), headers[i]);
            }
            
            response.Close();
        }
    }
}