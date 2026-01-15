using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPReceiver : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    volatile bool running;
    bool cleanedUp;

    public int port = 3001;

    private string latestDataRaw;
    private readonly object dataLock = new object();

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
        running = true;
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void ReceiveData()
    {
        try
        {
            client = new UdpClient(port);

            while (running)
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                string received = Encoding.UTF8.GetString(dataByte);

                lock (dataLock)
                {
                    latestDataRaw = received;
                }
            }
        }
        catch (SocketException)
        {
            // Expected during shutdown
        }
        catch
        {
            // NO Debug.Log here — threads must stay silent
        }
    }

    void Cleanup()
    {
        if (cleanedUp) return;
        cleanedUp = true;

        running = false;

        if (client != null)
        {
            client.Close();
            client = null;
        }

        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join(100);
            receiveThread = null;
        }
    }

    void OnDestroy()
    {
        if (Application.isPlaying)
            Cleanup();
    }

    void OnApplicationQuit()
    {
        Cleanup();
    }
}
