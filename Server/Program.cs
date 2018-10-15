using Server.Server;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {           
            // Формирование строки подключения к БД на основе параметром командной строки
            ConnectionStringBuilder.ParseArgs(args);

            // Асинхронный запуск TCP сервера
            TcpServer.Start();

            // Запуск обработчика команд консоли и блокирование основного потока
            CommandsHandler.Start();            
        }
    }
}
