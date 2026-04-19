using System;
using System.IO;
using System.Linq;

public class Commands
{
	private static readonly string folder = "files";
	public static string Execute (string command)
	{
		if (!Directory.Exists(folder))
			Directory.CreateDirectory(folder);

		string[] parts = command.Split('', 2);
		string cmd = parts[0].ToLower();

		switch (cmd)
		{ case "/list":
				return ListFiles();

			case "/read":
				if (parts.Length < 2) return "Jep emrin e file";
				return ReadFile(parts[1]);

			case "/delete":
				if (parts.Length<2) return "Jep emrin e file";
                return DeleteFile(parts[1])


                    case "/search":
                if (parts.Length < 2) return "Jep emrin e file";
                return SearchFile(parts[1])


                    case "/info":
                if (parts.Length < 2) return "Jep emrin e file";
                return InfoFile(parts[1])


					case "/upload":
				return "Upload do te trajtohet nga klienti";

            case "/download":
                return "Download do te trajtohet nga klienti";

			default:
				return "Komanda eshte e panjohur!";


        }
	}
}
