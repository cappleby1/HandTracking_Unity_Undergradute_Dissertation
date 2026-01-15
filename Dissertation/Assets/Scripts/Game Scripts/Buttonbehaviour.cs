using UnityEngine;

public class Buttonbehaviour : MonoBehaviour
{
    public GameObject[] ButtonCollection;
    public GameControllerScript GameController;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject obj in ButtonCollection)
        {
            Renderer SphereRenderer = obj.GetComponent<Renderer>();
            SphereRenderer.material.color = Color.red;
        }

    }

    void Update()
    {
    }
}
