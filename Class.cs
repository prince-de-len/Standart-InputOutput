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
    private Caretaker _caretaker = new Caretaker();

    public void SaveAsBinary(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(fileStream, this);
        }
    }

    public static TextFile LoadFromBinary(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (TextFile)binaryFormatter.Deserialize(fileStream);
        }
    }

    public void SaveAsXml(string filePath)
    {
        filePath = filePath.Substring(0, filePath.Length - 4) + ".xml";
        using (StreamWriter streamWriter = new StreamWriter(filePath))
        {
            var xmlSerializer = new XmlSerializer(typeof(TextFile));
            xmlSerializer.Serialize(streamWriter, this);
        }
    }

    public static TextFile LoadFromXml(string filePath)
    {
        filePath = filePath.Substring(0, filePath.Length - 4) + ".xml";
        using (StreamReader streamReader = new StreamReader(filePath))
        {
            var xmlSerializer = new XmlSerializer(typeof(TextFile));
            return (TextFile)xmlSerializer.Deserialize(streamReader);
        }
    }
    public void WriteToFile(string content)
    {
        Content = content;
    }

    // Memento:
    public void SaveState()
    {
        _caretaker.Memento = new Memento(this);
    }

    public void RestoreState()
    {
        if (_caretaker.Memento != null)
        {
            FileName = _caretaker.Memento.FileName;
            Content = _caretaker.Memento.Content;
        }
    }

}

public class TextSearch
{
    public List<string> SearchFilesByKeyword(string directoryPath, string keyword, string extension)
    {
        List<string> foundFiles = new List<string>();

        string[] files = Directory.GetFiles(directoryPath, "*." + extension, SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string content = File.ReadAllText(file);
            if (content.Contains(keyword))
            {
                foundFiles.Add(file);
            }
        }

        return foundFiles;
    }
}

[Serializable]
public class Memento 
{
    public string FileName { get; set; }
    public string Content { get; set; }

    public Memento(TextFile textFile)
    {
        FileName = textFile.FileName;
        Content = textFile.Content;
    }
}

[Serializable]
class Caretaker
{
    public Memento Memento { get; set; }
}

class KeywordInputOutput
{
    public string[] GetKeywordsFromConsole()
    {
        Console.WriteLine("Введите ключевые слова для индексации (для завершения ввода введите пустую строку):");

        var keywords = new List<string>();
        string input;

        do
        {
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                keywords.Add(input);
            }
        }
        while (!string.IsNullOrWhiteSpace(input));

        return keywords.ToArray();
    }
}

