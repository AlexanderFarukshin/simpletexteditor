using System;
using System.Collections.Generic;
using Server.Dictionary;
using Server.TextAnalyzer;

namespace Server
{
    public static class CommandsHandler
    {
        public static void Start()
        {
            IDictionaryManager dictionaryManager = new DictionaryManager();

            while (true)
            {
                try
                {
                    var command = Console.ReadLine();
                    var commandArgs = command.Split(' ');
                    var commandName = commandArgs[0];

                    string file = null;
                    Dictionary<string, int> frequentWords = null;

                    switch (commandName)
                    {
                        case "create":
                        case "update":
                            file = commandArgs[1];
                            frequentWords = TextFileAnalyzer.GetFrequentWords(file, 2, 3, 15);
                            switch (commandName)
                            {
                                case "create":
                                    dictionaryManager.Create(frequentWords);
                                    Console.WriteLine("Словарь успешно создан.");
                                    break;

                                case "update":
                                    dictionaryManager.Update(frequentWords);
                                    Console.WriteLine("Словарь успешно обновлен.");
                                    break;
                            }
                            break;

                        case "clear":
                            dictionaryManager.Clear();
                            Console.WriteLine("Словарь успешно отчищен.");
                            break;

                        default:
                            Console.WriteLine("Команда не найдена.");
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("При выполнении команды возникал ошибка.");
                }

                Console.WriteLine();
            }
        }
    }
}
