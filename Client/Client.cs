using System;
using System.Net.Sockets;
using System.Text;


namespace Client
{
    public static class Client
    {
        public static void Start(string[] args)
        {
            string ip = null;
            int port = 0;
            ParseArgs(args, ref ip, ref port);

            while (true)
            {
                var command = Console.ReadLine();

                if (string.IsNullOrEmpty(command)) continue;

                try
                {
                    var client = new TcpClient(ip, port);

                    using (var networkStream = client.GetStream())
                    {
                        SendRequestText(networkStream, command);

                        var responsetText = GetResponseText(networkStream);
                        var responseArgs = responsetText.Split('\n');
                        var responseHeader = responseArgs[0];
                        var responseBody = responseArgs[1];
                        var responseStatus = responseHeader.Split(' ')[1];

                        if (responseStatus == "OK")
                        {
                            var suitableWords = responseBody.Split(' ');
                            foreach (var word in suitableWords) Console.WriteLine("- {0}", word);
                        }
                        else
                        {
                            Console.WriteLine(responseBody);
                        }

                        Console.WriteLine();
                    }
                }
                catch
                {
                    Console.WriteLine("\nВозникла ошибка при работе с сервером.\n");    
                }              
            }
        }

        private static void ParseArgs(string[] args, ref string ip, ref int port)
        {
            try
            {

                for (var argNum = 0; argNum < args.Length; argNum++)
                {
                    switch (args[argNum])
                    {
                        case "-ip":
                            ip = args[argNum + 1];
                            break;

                        case "-port":
                            port = Int32.Parse(args[argNum + 1]);
                            break;
                    }
                }
            }
            catch
            {
                ip = null;
                port = 0;
            }
        }

        private static void SendRequestText(NetworkStream networkStream, string text)
        {
            var offset = 0;
            var buffer = Encoding.UTF8.GetBytes(text);
            networkStream.Write(buffer, offset, buffer.Length);
        }

        private static string GetResponseText(NetworkStream networkStream)
        {
            var offset = 0;
            var bufferSize = 1024;
            var buffer = new byte[bufferSize];
            networkStream.Read(buffer, offset, buffer.Length);

            var responseText = Encoding.UTF8.GetString(buffer);
            responseText = ClearResponsetText(responseText);

            return responseText;
        }

        private static string ClearResponsetText(string text)
        {
            var startIndex = 0;
            var endIndex = text.IndexOf('\0');
            return text.Substring(startIndex, endIndex);
        }
    }
}
