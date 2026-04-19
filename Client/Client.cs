using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

class Client
{
    static void Main()
    {
        Console.Write("IP e serverit: ");
        string serverIp = Console.ReadLine();

        int port = 5000;

        UdpClient client = new UdpClient();
        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIp), port);

        Console.WriteLine("Shkruaj komanda (/list, /read, /upload, /download, /login admin)");

        while (true)
        {
            Console.Write("> ");
            string cmd = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cmd))
                continue;

          
            if (cmd.StartsWith("/upload"))
            {
                string[] parts = cmd.Split(' ');
                if (parts.Length < 2)
                {
                    Console.WriteLine("Jep filename!");
                    continue;
                }

                UploadFile(client, serverEP, parts[1]);
                continue;
            }

           
            if (cmd.StartsWith("/download"))
            {
                string[] parts = cmd.Split(' ');
                if (parts.Length < 2)
                {
                    Console.WriteLine("Jep filename!");
                    continue;
                }

                DownloadFile(client, serverEP, parts[1]);
                continue;
            }

           
            byte[] data = Encoding.UTF8.GetBytes(cmd);
            client.Send(data, data.Length, serverEP);

            // merr përgjigje
            IPEndPoint ep = null;
            byte[] res = client.Receive(ref ep);

            Console.WriteLine(Encoding.UTF8.GetString(res));
        }
    }

    
    static void UploadFile(UdpClient client, IPEndPoint serverEP, string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("File nuk ekziston!");
            return;
        }

        
        byte[] cmd = Encoding.UTF8.GetBytes("/upload");
        client.Send(cmd, cmd.Length, serverEP);

      
        byte[] name = Encoding.UTF8.GetBytes(Path.GetFileName(filename));
        client.Send(name, name.Length, serverEP);

       
        using (FileStream fs = new FileStream(filename, FileMode.Open))
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                client.Send(buffer, bytesRead, serverEP);
            }
        }

       
        byte[] eof = Encoding.UTF8.GetBytes("EOF");
        client.Send(eof, eof.Length, serverEP);

        Console.WriteLine("Upload u kry!");
    }

    
    static void DownloadFile(UdpClient client, IPEndPoint serverEP, string filename)
    {
        byte[] cmd = Encoding.UTF8.GetBytes($"/download {filename}");
        client.Send(cmd, cmd.Length, serverEP);

        IPEndPoint ep = null;

        // merr emrin
        byte[] nameData = client.Receive(ref ep);
        string name = Encoding.UTF8.GetString(nameData);

        if (name == "File nuk ekziston")
        {
            Console.WriteLine(name);
            return;
        }

        using (FileStream fs = new FileStream("download_" + name, FileMode.Create))
        {
            while (true)
            {
                byte[] chunk = client.Receive(ref ep);
                string chunkStr = Encoding.UTF8.GetString(chunk);

                if (chunkStr == "EOF")
                    break;

                fs.Write(chunk, 0, chunk.Length);
            }
        }

        Console.WriteLine("Download u kry!");
    }
}

