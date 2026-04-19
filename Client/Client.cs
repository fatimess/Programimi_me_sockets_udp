using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        string serverIP = "127.0.0.1";
        int port = 5000;

        UdpClient client = new UdpClient();
        client.Client.ReceiveTimeout = 5000;

        IPEndPoint server = new IPEndPoint(IPAddress.Parse(serverIP), port);

      
        Console.Write("Zgjedh rolin (admin/user): ");
        string role = Console.ReadLine().ToLower();

        if (role != "admin" && role != "user")
        {
            Console.WriteLine("Rol i pavlefshëm!");
            return;
        }

       
        string roleMsg = "ROLE:" + role.ToUpper();
        byte[] roleData = Encoding.UTF8.GetBytes(roleMsg);
        client.Send(roleData, roleData.Length, server);

        Console.WriteLine($"\nJe kyçur si {role.ToUpper()}");

        if (role == "admin")
        {
            Console.WriteLine("Komandat:");
            Console.WriteLine("/list");
            Console.WriteLine("/read <file>");
            Console.WriteLine("/upload <file>");
            Console.WriteLine("/download <file>");
            Console.WriteLine("/delete <file>");
            Console.WriteLine("/search <keyword>");
            Console.WriteLine("/info <file>");
        }

        while (true)
        {
            Console.Write("\nShkruaj: ");
            string msg = Console.ReadLine();

            if (msg == "quit") break;

          
            if (role == "user" && msg.StartsWith("/"))
            {
                Console.WriteLine("Nuk ke privilegje për këtë komandë!");
                continue;
            }

            byte[] data = Encoding.UTF8.GetBytes(msg);
            client.Send(data, data.Length, server);

            try
            {
                IPEndPoint remote = null;
                byte[] response = client.Receive(ref remote);

                string responseMsg = Encoding.UTF8.GetString(response);
                Console.WriteLine("[SERVER]: " + responseMsg);
            }
            catch
            {
                Console.WriteLine("Serveri nuk po përgjigjet!");
            }
        }

        client.Close();
    }
}

