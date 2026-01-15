using UnityEngine;

public class IndividualButtonBehaviour : MonoBehaviour
{
    private Renderer ButtonRenderer;
    public GameControllerScript GameController;

    private void OnTriggerEnter()
    {
        ButtonRenderer = GetComponent<Renderer>();
        if(ButtonRenderer.material.color != Color.green)
        {
            ButtonRenderer.material.color = Color.green;
            GameController.CountIncrease();
        }
    }
}
