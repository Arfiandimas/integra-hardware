using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPListener
{
    private const int listenPort = 8888;

    private static void StartListener()
    {
        UdpClient listener = new UdpClient(listenPort);
        IPAddress ipaddress = IPAddress.Parse("192.168.1.23");
        IPEndPoint groupEP = new IPEndPoint(ipaddress, listenPort);
        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);
                if(Encoding.ASCII.GetString(bytes, 0, bytes.Length) != "")
                {
                    ReturnResponse();
                } else {
                    ReturnResponse("failed");
                }
            }
        } catch (SocketException e) {
            Console.WriteLine(e);
        } finally {
            listener.Close();
        }
    }

    static void ReturnResponse(string message = "success")
    {
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);
        IPAddress serverAddr = IPAddress.Parse("192.168.1.23");
        IPEndPoint endPoint = new IPEndPoint(serverAddr, 8888);
        byte[] send_buffer = Encoding.ASCII.GetBytes("success");
        sock.SendTo(send_buffer, endPoint);
    }

    public static void Main()
    {
        StartListener();
    }
}