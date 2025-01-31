using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReciever : MonoBehaviour
{
    Thread thread;
    UdpClient client; 
    public int port = 3001;
    public bool startRecieving = true;
    public string data;


    public void Start()
    {
        thread = new Thread(new ThreadStart(ReceiveData));
        thread.Start();
    }


    // Receieve Data
    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (startRecieving)
        {
            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
            byte[] dataByte = client.Receive(ref anyIP);
            data = Encoding.UTF8.GetString(dataByte);
        }
    }
}
