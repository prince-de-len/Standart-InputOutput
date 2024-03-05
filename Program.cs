using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

class Program
{
    static void Main()
    {
        string directoryPath = "C:\\Users\\princedelen\\source\\repos\\Standart InputOutput";
        string keyword;
        string content;
        string extension;

        Console.WriteLine("Добро пожаловать в творческую лабораторию имени Иосифа Виссарионовича Сталина! " +
            "Чтобы вы не запутались, введу вас в курс дела: сначала программа вас попросит указать ключевые слова для " +
            "индексации, потом попросит одно ключевое слово для поиска текстовых файлов по слову, в данной связи тут же попросит " +
            "вписать расширение файлов, которые вы ищете для поиска файлов (не индексации!). Далее программа попросит вас " +
            "вписать что-нибудь в файл example.txt, на основе коего демонстрируется функциональность сей изваяния человеческого " +
            "разумения. Далее вы узрите возможности паттерна Memento. Удачи!\n");

        KeywordInputOutput keywordInputOutput = new KeywordInputOutput();
        string[] keywords = keywordInputOutput.GetKeywordsFromConsole();

        Console.WriteLine("Введите ключевое слово для поиска файла:");
        keyword = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Файлы с какими расширениями (bin, xml, txt)?");
        extension = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Введите содержание файла:");
        content = Console.ReadLine();
        Console.WriteLine();

        TextFile textFile = new TextFile
        {
            FileName = "example.txt",
        };

        textFile.WriteToFile(content);

        textFile.SaveAsBinary(directoryPath + "\\example.bin");

        var loadedFromFile = TextFile.LoadFromBinary(directoryPath + "\\example.bin");
        Console.WriteLine("Содержимое из бинарного файла: " + loadedFromFile.Content);

        textFile.SaveAsXml(directoryPath + textFile.FileName);

        var loadedFromXml = TextFile.LoadFromXml(directoryPath + textFile.FileName);
        Console.WriteLine("Содержимое из XML файла: " + loadedFromXml.Content);

        TextSearch SearchResult = new TextSearch();
        List<string> ResultOfSearch = SearchResult.SearchFilesByKeyword(directoryPath, keyword, extension);
        foreach (string Item in ResultOfSearch)
        {
            Console.WriteLine(Item);
        }

        Console.WriteLine();
        IndexFiles(directoryPath, keywords); // Индексация

        // memento
        textFile.SaveState(); // Сохраняем состояние

        Console.WriteLine("\nТекущее состояние:");
        Console.WriteLine($"FileName: {textFile.FileName}, Content: {textFile.Content}");


        textFile.FileName = "updated.txt";
        textFile.Content = "Updated content";
        Console.WriteLine("\nНовое состояние:");
        Console.WriteLine($"FileName: {textFile.FileName}, Content: {textFile.Content}");

        textFile.RestoreState(); // Восстанавливаем состояние
        Console.WriteLine("\nВосстановленное состояние:");
        Console.WriteLine($"FileName: {textFile.FileName}, Content: {textFile.Content}");
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
