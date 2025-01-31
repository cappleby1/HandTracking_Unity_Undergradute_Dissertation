using UnityEngine;

public class HandTracking : MonoBehaviour
{
    public UDPReciever uDPReciever;
    public GameObject[] handPoints;

    // Update is called once per frame
    void Update()
    {
        // Receieve the data
        string data = uDPReciever.data;
        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);

        string[] points = data.Split(',');

        // Calculate the position of the points
        for (int i = 0; i<21; i++)
        {
            float x = 7-float.Parse(points[i*3])/30;
            float y = float.Parse(points[i*3+1])/30;
            float z = float.Parse(points[i*3+2])/30;

            handPoints[i].transform.localPosition = new Vector3(x, y, z);
        }
    }
}
