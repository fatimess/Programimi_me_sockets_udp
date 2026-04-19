using System;
using System.Net;
using System.Text;
using System.Threading;

public class HttpServer
{
    public static void Start()
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");
        listener.Start();

        Console.WriteLine("HTTP server ne 8080");

        new Thread(() =>
        {
            while (true)
            {
                var ctx = listener.GetContext();
                var res = ctx.Response;

                string json = $@"{{
    ""clients"": {ServerCore.clients.Count},
    ""messages"": {ServerHandler.totalMessages}
}}";

                byte[] buffer = Encoding.UTF8.GetBytes(json);
                res.OutputStream.Write(buffer, 0, buffer.Length);
                res.Close();
            }
        }).Start();
    }
    
}
