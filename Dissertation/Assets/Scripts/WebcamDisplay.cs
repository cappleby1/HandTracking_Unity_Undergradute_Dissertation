using UnityEngine;
using UnityEngine.UI;

public class WebcamDisplay : MonoBehaviour
{
    private WebCamTexture webcamTexture;

    void Start()
    {
        // Get the available devices and choose the first one
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            // Set up the webcam texture & render it
            webcamTexture = new WebCamTexture(devices[0].name);
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.mainTexture = webcamTexture;

            // Flip the camera horizontally so it is not reversed
            Vector2[] uv = GetComponent<MeshFilter>().mesh.uv;
            for (int i = 0; i < uv.Length; i++)
            {
                uv[i] = new Vector2(1f - uv[i].x, uv[i].y);
            }
            GetComponent<MeshFilter>().mesh.uv = uv;

            webcamTexture.Play(); 
        }
    }
}
