

namespace Server
{
    public static class ConnectionStringBuilder
    {
        private static string _ip = "127.0.0.1";
        private static string _port = "1433";
        private static string _database = "TestDB";
        private static string _user = "sa";
        private static string _password = "1";

        public static void ParseArgs(string[] args)
        {    
            for (var argNum = 0; argNum < args.Length; argNum++)
            {
                switch (args[argNum])
                {
                    case "-ip":
                        _ip = args[argNum + 1];
                        break;

                    case "-port":
                        _port = args[argNum + 1];
                        break;
                }
            }            
        }

        public static string GetConnectionString()
        {      
            return string.Format("Server = {0},{1}; Database = {2}; User Id = {3}; Password = {4};", _ip, _port, _database, _user, _password);
        }
    }
}
