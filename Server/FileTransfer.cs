using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class FileTransfer
{
    private static readonly string folder = "files";

    public static string ReceiveFile(UdpClient server, IPEndPoint clientEP)
    {
        byte[] nameData = server.Receive(ref clientEP);
        string filename = Encoding.UTF8.GetString(nameData);

        string path = Path.Combine(folder, filename);

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            while (true)
            {
                byte[] chunk = server.Receive(ref clientEP);
                string str = Encoding.UTF8.GetString(chunk);

                if (str == "EOF") break;

                fs.Write(chunk, 0, chunk.Length);
            }
        }

        return "Upload OK";
    }

    public static void SendFile(UdpClient server, IPEndPoint clientEP, string filename)
    {
        string path = Path.Combine(folder, filename);

        if (!File.Exists(path))
        {
            byte[] err = Encoding.UTF8.GetBytes("File nuk ekziston");
            server.Send(err, err.Length, clientEP);
            return;
        }

        byte[] name = Encoding.UTF8.GetBytes(filename);
        server.Send(name, name.Length, clientEP);

        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            byte[] buffer = new byte[1024];
            int bytes;

            while ((bytes = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                server.Send(buffer, bytes, clientEP);
            }
        }

        server.Send(Encoding.UTF8.GetBytes("EOF"), 3, clientEP);
    }
}
