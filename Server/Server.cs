using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

class ServerCore
{
    public static UdpClient server = null!;
    public static int port = 5000;

    public static ConcurrentDictionary<string, DateTime> clients = new();
    public static int timeoutSeconds = 30;

    public static void Start()
    {
        server = new UdpClient(port);
        Console.WriteLine($"Server duke degjuar ne port {port}");

        new Thread(CheckClients).Start();

        while (true)
        {
            IPEndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = server.Receive(ref clientEP);

            string msg = Encoding.UTF8.GetString(data);
            string key = clientEP.ToString();

            clients[key] = DateTime.Now;

            Console.WriteLine($"[{key}] -> {msg}");

            ServerHandler.Handle(msg, clientEP, key);
        }
    }

    static void CheckClients()
    {
        while (true)
        {
            foreach (var c in clients)
            {
                if ((DateTime.Now - c.Value).TotalSeconds > timeoutSeconds)
                {
                    Console.WriteLine($"Klienti {c.Key} u largua");
                    clients.TryRemove(c.Key, out _);
                }
            }

            Thread.Sleep(5000);
        }
    }
}
