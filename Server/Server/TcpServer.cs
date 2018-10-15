using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.Dictionary;

namespace Server.Server
{
    public static class TcpServer
    {
        public static void Start()
        {
            Task.Factory.StartNew(StartListener);
        }

        private static void StartListener()
        {
            var ip = "127.0.0.1";
            var port = 2715;

            var listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();

            while (true)
            {
                var client = listener.AcceptTcpClient();
                Task.Factory.StartNew(() => HandleClient(client));
            }
        }

        private static void HandleClient(TcpClient client)
        {
            try
            {
                using (var networkStream = client.GetStream())
                {
                    var requestText = GetRequestText(networkStream);
                    var responseText = GetResponseText(requestText);
                    SendResponseText(networkStream, responseText);
                }
            }
            catch
            {
                // Продолжить работу
            }
            finally
            {
                client.Close();
            }
        }

        private static string GetRequestText(NetworkStream networkStream)
        {
            var bufferSize = 1024;
            var offset = 0;
            var buffer = new byte[bufferSize];
            networkStream.Read(buffer, offset, bufferSize);

            var request = Encoding.UTF8.GetString(buffer);
            request = ClearRequestText(request);

            return request;
        }

        private static string ClearRequestText(string text)
        {
            var startIndex = 0;
            var endIndex = text.IndexOf('\0');
            return text.Substring(startIndex, endIndex);
        }

        private static string GetResponseText(string request)
        {
            string responseText;

            try
            {              
                var commandArgs = request.Split(' ');
                var commandName = commandArgs[0];

                var prefix = commandName == "get" ? commandArgs[1] : null;
                if (prefix == null) throw new Exception();
                
                IDictionaryManager dictionaryManager = new DictionaryManager();
                var maxWordsCount = 5;
                var suitableWords = dictionaryManager.GetSuitableWords(prefix, maxWordsCount);

                responseText =
                    "Status OK\n" +
                    suitableWords.Aggregate((x, y) => x + " " + y);
            }
            catch
            {
                responseText =
                   "Status ERROR\n" +
                   "Ошибка обработки запроса.";
            }

            return responseText;
        }

        private static void SendResponseText(NetworkStream networkStream, string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            var offset = 0;
            networkStream.Write(buffer, offset, buffer.Length);
        }
    }
}
