using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPReceiver : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;

    public int port = 3001;
    private string latestDataRaw;
    private object dataLock = new object();

    public string data
    {
        get
        {
            lock (dataLock)
            {
                return latestDataRaw;
            }
        }
    }

    void Start()
    {
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // Try to receieve the coords
    void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                string received = Encoding.UTF8.GetString(dataByte);

                // Lock to stop issues from receiving
                lock (dataLock)
                {
                    latestDataRaw = received;
                }
            }

            catch 
            {
                debug.log("Problems receiving data")
            }
        }
    }

    // Close everything on quit / Clean up
    void OnApplicationQuit()
    {
        if (client != null) client.Close();
        if (receiveThread != null && receiveThread.IsAlive) receiveThread.Abort();
    }
}
