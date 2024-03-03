using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

class Program
{
    static void Main()
    {
        Console.WriteLine("Добро пожаловать в творческую лабораторию имени Иосифа Виссарионовича Сталина!" +
            "Чтобы вы не запутались, введу вас в курс дела: сначала программа вас попросит указать ключевые слова для " +
            "индексации, потом попросит одно ключевое слово для поиска текстовых файлов по слову, в данной связи тут же попросит" +
            "вписать расширение файлов, которые вы ищете для поиска файлов (не индексации!). Далее программа попросит вас " +
            "вписать что-нибудь в файл example.txt, на основе коего демонстрируется функциональность сей изваяния человеческого" +
            "разумения. Далее вы узрите возможности паттерна Memento. Удачи!\n");
        string DirectoryPath = "C:\\Users\\princedelen\\source\\repos\\Standart InputOutput";
        string Keyword;
        string Content;
        string Extension;

        KeywordInputOutput KeywordInputOutput = new KeywordInputOutput();
        string[] Keywords = KeywordInputOutput.GetKeywordsFromConsole();

        Console.WriteLine("Введите ключевое слово для поиска файла:");
        Keyword = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Файлы с какими расширениями (bin, xml, txt)?");
        Extension = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Введите содержание файла:");
        Content = Console.ReadLine();
        Console.WriteLine();

        TextFile File = new TextFile
        {
            FileName = "example.txt",
        };

        File.WriteToFile(Content);

        File.SaveAsBinary(DirectoryPath + "\\example.bin");

        var LoadedFromFile = TextFile.LoadFromBinary(DirectoryPath + "\\example.bin");
        Console.WriteLine("Содержимое из бинарного файла: " + LoadedFromFile.Content);

        File.SaveAsXml(DirectoryPath + "\\example.xml");

        var LoadedFromXml = TextFile.LoadFromXml(DirectoryPath + "\\example.xml");
        Console.WriteLine("Содержимое из XML файла: " + LoadedFromXml.Content);

        TextSearch SearchResult = new TextSearch();
        List<string> ResultOfSearch = SearchResult.SearchFilesByKeyword(DirectoryPath, Keyword, Extension);
        foreach (string Item in ResultOfSearch)
        {
            Console.WriteLine(Item);
        }

        Console.WriteLine();
        IndexFiles(DirectoryPath, Keywords); // Индексация

        // memento

        File.SaveState(); // Сохраняем состояние

        Console.WriteLine("\nТекущее состояние:");
        Console.WriteLine($"FileName: {File.FileName}, Content: {File.Content}");


        File.FileName = "updated.txt";
        File.Content = "Updated content";
        Console.WriteLine("\nНовое состояние:");
        Console.WriteLine($"FileName: {File.FileName}, Content: {File.Content}");

        File.RestoreState(); // Восстанавливаем состояние
        Console.WriteLine("\nВосстановленное состояние:");
        Console.WriteLine($"FileName: {File.FileName}, Content: {File.Content}");
    }
    
    // Приложение для индексации
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
