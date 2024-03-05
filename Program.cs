using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

class Program
{
    static void Main()
    {
        bool running = true;
        TextFile textFile = new TextFile
        {
            FileName = "example.txt",
        };
        string directoryPath = "C:\\Users\\princedelen\\source\\repos\\Standart InputOutput";
        string keyword;
        string content;
        string extension;
        ConsoleKeyInfo key;
        int counterKeyEnter = 0;
        bool completeFirstRun = false;

        Console.WriteLine("Добро пожаловать в творческую лабораторию имени Иосифа Виссарионовича Сталина! " +
            "Чтобы вы не запутались, введу вас в курс дела:\n\n" +
            "Для начала программы, нажмите enter!\n");

        while (running)
        {
            key = Console.ReadKey();

            Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r"); // шобы не было видно ввод цифры

            if (key.Key != ConsoleKey.Enter && !completeFirstRun)
            {
                Console.WriteLine("Неверно :(");
                running = false;
                break;
            }

            switch (key.KeyChar)
            {
                case '1':
                    Console.WriteLine("Введите ключевое слово для поиска файла (например, \"example\" (плеоназм)):");
                    keyword = Console.ReadLine();
                    Console.WriteLine();

                    Console.WriteLine("Файлы с какими расширениями (bin, xml, txt)?");
                    extension = Console.ReadLine();
                    Console.WriteLine();

                    Console.WriteLine("\nПоиск файлов:");
                    TextSearch searchResult = new TextSearch();
                    List<string> resultOfSearch = searchResult.SearchFilesByKeyword(directoryPath, keyword, extension);
                    foreach (string item in resultOfSearch)
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case '2':
                    Console.WriteLine("Введите содержание файла:");
                    content = Console.ReadLine();

                    textFile.WriteToFile(content);

                    break;
                case '3':
                    textFile.SaveAsBinary(directoryPath + "\\example.bin");

                    var loadedFromFile = TextFile.LoadFromBinary(directoryPath + "\\example.bin");
                    Console.WriteLine("\nСодержимое из бинарного файла: " + loadedFromFile.Content);

                    textFile.SaveAsXml(directoryPath + textFile.FileName);

                    var loadedFromXml = TextFile.LoadFromXml(directoryPath + textFile.FileName);
                    Console.WriteLine("Содержимое из XML файла: " + loadedFromXml.Content);

                    break;
                case '4':
                    // memento
                    Console.WriteLine("\nТекущее состояние:");
                    Console.WriteLine($"FileName: {textFile.FileName}, Content: {textFile.Content}");

                    Caretaker caretaker = new Caretaker();
                    caretaker.SaveState(textFile); // Сохраняем состояние

                    textFile = new TextFile
                    {
                        FileName = "updated.txt",
                        Content = "updated content",
                    };

                    Console.WriteLine("\nНовое состояние:");
                    Console.WriteLine($"FileName: {textFile.FileName}, Content: {textFile.Content}");

                    caretaker.RestoreState(textFile); // Восстанавливаем состояние
                    Console.WriteLine("\nВосстановленное состояние:");
                    Console.WriteLine($"FileName: {textFile.FileName}, Content: {textFile.Content}");
                    
                    break;
                case '5':
                    KeywordInputOutput keywordInputOutput = new KeywordInputOutput();
                    string[] keywords = keywordInputOutput.GetKeywordsFromConsole();

                    // Индексация
                    IndexFiles(directoryPath, keywords);

                    break;
                case '6':
                    Console.WriteLine("Введите новое имя файла (без расширения!)");
                    textFile.FileName = Console.ReadLine();

                    break;
                case '7':
                    Console.WriteLine("Выход из программы...");
                    running = false;
                    break;
                default:
                    if (key.Key == ConsoleKey.Enter)
                    {
                        ++counterKeyEnter;
                        if (counterKeyEnter < 3)
                        {
                            Console.WriteLine("Программа запущена!");
                        }
                        else if (3 <= counterKeyEnter && counterKeyEnter < 6)
                        {
                            Console.WriteLine("Да работает, работает!");
                        }
                        else if (6 <= counterKeyEnter && counterKeyEnter < 9)
                        {
                            Console.WriteLine("Работает, видишь же!");
                        }
                        else if (9 <= counterKeyEnter && counterKeyEnter <= 12)
                        {
                            Console.WriteLine("Может хватит, а?");
                        }
                        else
                        {
                            running = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор");
                    }
                    break;
            }
            Console.WriteLine("\n1. Поиск файла\n" +
            "2. Ввод содержания файла\n" +
            "3. Вывод содержания файла\n" +
            "4. Затестить memento паттерн\n" +
            "5. Индексация\n" +
            "6. Переименовать файл\n" +
            "7. Выход\n");
            completeFirstRun = true;
        }
    }

    
    // функция для индексации
    static void IndexFiles(string directoryPath, string[] keywords)
    {
        if (Directory.Exists(directoryPath))
        {
            var files = Directory.GetFiles(directoryPath, "*.txt", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                Console.WriteLine($"Индексация файла: {file}");
                var content = File.ReadAllText(file);

                foreach (var keyword in keywords)
                {
                    if (content.Contains(keyword))
                    {
                        Console.WriteLine($"Ключевое слово '{keyword}' найдено в файле: {file}");
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Указанная директория не существует.");
        }
    }
}
