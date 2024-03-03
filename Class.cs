using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

[Serializable] 
public class TextFile
{
    public string FileName { get; set; }
    public string Content { get; set; }
    private TextFileMemento memento;


    public void SaveAsBinary(string filePath)
    {
        using (FileStream FileStream = new FileStream(filePath, FileMode.Create))
        {
            var BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            BinaryFormatter.Serialize(FileStream, this);
        }
    }

    public static TextFile LoadFromBinary(string filePath)
    {
        using (FileStream FileStream = new FileStream(filePath, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (TextFile)binaryFormatter.Deserialize(FileStream);
        }
    }

    public void SaveAsXml(string filePath)
    {
        using (StreamWriter StreamWriter = new StreamWriter(filePath))
        {
            var XmlSerializer = new XmlSerializer(typeof(TextFile));
            XmlSerializer.Serialize(StreamWriter, this);
        }
    }

    public static TextFile LoadFromXml(string filePath)
    {
        using (StreamReader StreamReader = new StreamReader(filePath))
        {
            var XmlSerializer = new XmlSerializer(typeof(TextFile));
            return (TextFile)XmlSerializer.Deserialize(StreamReader);
        }
    }
    public void WriteToFile(string content)
    {
        Content = content;
    }

    // Memento:
    public void SaveState()
    {
        memento = new TextFileMemento(this);
    }

    public void RestoreState()
    {
        if (memento != null)
        {
            FileName = memento.FileName;
            Content = memento.Content;
        }
    }

}

public class TextSearch
{
    public List<string> SearchFilesByKeyword(string directoryPath, string keyword, string extension)
    {
        List<string> FoundFiles = new List<string>();

        string[] files = Directory.GetFiles(directoryPath, "*." + extension, SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string content = File.ReadAllText(file);
            if (content.Contains(keyword))
            {
                FoundFiles.Add(file);
            }
        }

        return FoundFiles;
    }
}

[Serializable]
public class TextFileMemento 
{
    public string FileName { get; set; }
    public string Content { get; set; }

    public TextFileMemento(TextFile textFile)
    {
        FileName = textFile.FileName;
        Content = textFile.Content;
    }
}

class KeywordInputOutput
{
    public string[] GetKeywordsFromConsole()
    {
        Console.WriteLine("Введите ключевые слова для индексации (для завершения ввода введите пустую строку):");

        var Keywords = new List<string>();
        string Input;

        do
        {
            Input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(Input))
            {
                Keywords.Add(Input);
            }
        }
        while (!string.IsNullOrWhiteSpace(Input));

        return Keywords.ToArray();
    }
}

