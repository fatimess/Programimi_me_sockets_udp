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

        string[] parts = command.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string cmd = parts[0].ToLower();

        switch (cmd)
        {
            case "/list":
                return ListFiles();

            case "/read":
                if (parts.Length < 2) return "Jep emrin e file!";
                return ReadFile(parts[1]);

            case "/delete":
                if (parts.Length < 2) return "Jep emrin e file!";
                return DeleteFile(parts[1]);

            case "/search":
                if (parts.Length < 2) return "Jep keyword!";
                return Search(parts[1]);

            case "/info":
                if (parts.Length < 2) return "Jep emrin e file!";
                return Info(parts[1]);

            case "/upload":
                return "Upload do te trajtohet nga klienti";

            case "/download":
                return "Download do te trajtohet nga klienti";

            default:
                return "Komanda eshte e panjohur!";
        }
    }

    private static string ListFiles()
    {
        var files = Directory.GetFiles(folder)
            .Select(Path.GetFileName);

        return files.Any() ? string.Join("\n", files) : "Nuk ka file";
    }

    private static string ReadFile(string name)
    {
        string path = Path.Combine(folder, name);

        if (!File.Exists(path))
            return "File nuk ekziston!";

        return File.ReadAllText(path);
    }

    private static string DeleteFile(string name)
    {
        string path = Path.Combine(folder, name);

        if (!File.Exists(path))
            return "File nuk ekziston!";

        File.Delete(path);
        return "File u fshi!";
    }

    private static string Search(string keyword)
    {
        var files = Directory.GetFiles(folder)
            .Select(Path.GetFileName)
            .Where(name => name != null &&
                           name.Contains(keyword, StringComparison.OrdinalIgnoreCase));

        return files.Any() ? string.Join("\n", files) : "Nuk u gjet asgje.";
    }

    private static string Info(string name)
    {
        string path = Path.Combine(folder, name);

        if (!File.Exists(path))
            return "File nuk ekziston!";

        FileInfo f = new FileInfo(path);

        return $"Emri: {f.Name}\nMadhesia: {f.Length} bytes\nKrijuar: {f.CreationTime}";
    }
}
