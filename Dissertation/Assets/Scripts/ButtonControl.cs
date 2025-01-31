using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public Material material;
    public Color collisionColor = Color.green;
    Rigidbody rb;
    private Renderer rend;
    public bool collisionDetected = false;
    public LevelController LevelController;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        rb.velocity = Vector3.zero;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!collisionDetected)
        {
            collisionDetected = true;
            rend.material.color = collisionColor;
            LevelController.UpdateTotal(1);
        }
    }
}
