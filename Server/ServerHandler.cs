using System;
using System.Net;
using System.Text;
using System.Collections.Concurrent;

public class ServerHandler
{
    public static ConcurrentDictionary<string, string> roles = new();
    public static int totalMessages = 0;

    public static void Handle(string msg, IPEndPoint clientEP, string key)
    {
        roles.TryAdd(key, "read");
        totalMessages++;

        string? response = null;

        if (msg.StartsWith("/login admin"))
        {
            roles[key] = "admin";
            response = "Admin OK";
        }
        else if (msg.StartsWith("/upload"))
        {
            if (roles[key] != "admin")
                response = "Ske privilegje!";
            else
                response = FileTransfer.ReceiveFile(ServerCore.server, clientEP);
        }
        else if (msg.StartsWith("/download"))
        {
            string[] p = msg.Split(' ');
            if (p.Length < 2)
                response = "Jep filename!";
            else
                FileTransfer.SendFile(ServerCore.server, clientEP, p[1]);
        }
        else if (msg.StartsWith("/delete") && roles[key] != "admin")
        {
            response = "Vetem admin!";
        }
        else
        {
            response = Commands.Execute(msg);
        }

        if (response != null)
        {
            byte[] res = Encoding.UTF8.GetBytes(response);
            ServerCore.server.Send(res, res.Length, clientEP);
        }
    }
}