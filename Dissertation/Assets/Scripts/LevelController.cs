using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public int buttonTotal;
    public bool isPaused;
    public GameObject levelCompleteUI;
    public Text displayTime;
    public Text timerText;
    private float startTime;
    public float finalTime;
    public float currentTime;
    public GameObject gamePlay;
    public GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        levelCompleteUI.SetActive(false);
        buttonTotal = 5;
        startTime = Time.time;
    }

    public void Update()
    {
        if(isPaused == false)
        {
        currentTime = Time.time - startTime;

        string minutes = ((int)currentTime / 60).ToString("00");
        string seconds = (currentTime % 60).ToString("00");

        timerText.text = minutes + ":" + seconds;
        }
        if (buttonTotal == 0)
        {
            isPaused = true;
            gamePlay.SetActive(false);
            finalTime = currentTime;
            displayTime.text = finalTime.ToString("");
            levelCompleteUI.SetActive(true);
        }

    }

    public void UpdateTotal(int amount)
    {
        buttonTotal -= amount;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
