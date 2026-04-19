using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

class HttpServer

{
    public static void Start()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8080);
        server.Start();

        Console.WriteLine("HTTP Server eshte startuar ne portin 8080 ");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Request: \n" + request);
            string response = HandleRequest(request);

            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);

            client.Close();
        }
    }

    private static string HandleRequest(string request)
    {
        string[] lines = request.Split("\r\n");
        string requestLine = lines[0];

        string[] parts = requestLine.Split(' ');
        string method = parts[0];
        string path = parts[1];


        if (method == "GET" && path == "/stats")
        {
           var data = new
           {
                clients = 2,
                ips = new string[] {"127.0.0.1"},
                messages = new string[] {"hello" , "test"}
           } ;

           string json = JsonSerializer.Serialize(data);

           return
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: application/json\r\n" +
                $"Content-Length: {json.Length}\r\n" +
                "Connection: close\r\n" +
                "\r\n" +
    json;
        }
        else
        {
            string json = JsonSerializer.Serialize(new {error = "Nuk ekziston"});

            return
                 "HTTP/1.1 404 Nuk ekziston\r\n" +
                "Content-Type: application/json\r\n" +
                $"Content-Length: {json.Length}\r\n" +
                "\r\n" +
                json;
        }
    }

}
