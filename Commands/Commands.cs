using System;
using System.IO;
using System.Linq;

public class Commands
{
    private static readonly string folder = "files";

    public static string Execute(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            return "Komanda eshte bosh!";

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string[] parts = command.Split(' ', 2);
        string cmd = parts[0].ToLower();

        switch (cmd)
        {
            case "/list":
                return ListFiles();

            case "/read":
                return parts.Length < 2 ? "Jep filename!" : ReadFile(parts[1]);

            case "/delete":
                return parts.Length < 2 ? "Jep filename!" : DeleteFile(parts[1]);

            case "/search":
                return parts.Length < 2 ? "Jep keyword!" : Search(parts[1]);

            case "/info":
                return parts.Length < 2 ? "Jep filename!" : Info(parts[1]);

            case "/upload":
                return "Upload do te trajtohet nga klienti";

            case "/download":
                return "Download do te trajtohet nga klienti";

            default:
                return "Komande e panjohur!";
        }
    }

    static string ListFiles()
    {
        var files = Directory.GetFiles(folder)
            .Select(Path.GetFileName);

        return files.Any() ? string.Join("\n", files) : "Nuk ka file";
    }

    static string ReadFile(string name)
    {
        string path = Path.Combine(folder, name);
        return File.Exists(path) ? File.ReadAllText(path) : "File nuk ekziston!";
    }

    static string DeleteFile(string name)
    {
        string path = Path.Combine(folder, name);

        if (!File.Exists(path))
            return "File nuk ekziston!";

        File.Delete(path);
        return "File u fshi!";
    }

    static string Search(string keyword)
    {
        var files = Directory.GetFiles(folder)
            .Select(Path.GetFileName)
            .Where(f => f != null && f.Contains(keyword, StringComparison.OrdinalIgnoreCase));

        return files.Any() ? string.Join("\n", files) : "Nuk u gjet asgje";
    }

    static string Info(string name)
    {
        string path = Path.Combine(folder, name);

        if (!File.Exists(path))
            return "File nuk ekziston!";

        FileInfo f = new FileInfo(path);

        return $"Emri: {f.Name}\nMadhesia: {f.Length} bytes\nKrijuar: {f.CreationTime}\nModifikuar: {f.LastWriteTime}";
    }
}