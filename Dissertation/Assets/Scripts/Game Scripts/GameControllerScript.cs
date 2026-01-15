using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameControllerScript : MonoBehaviour
{
    public GameObject LevelCompleteMenu;
    public int ButtonCount;
    bool LevelCompleted = false;

    public bool TimerRunning = false;
    public float Timer;
    public TMP_Text TimerText;
    public TMP_Text FinalGameTime;

    void Start()
    {
        LevelCompleteMenu.SetActive(false);
        ButtonCount = 0;
        TimerRunning = true;
    }

    void Update()
    {

        if(ButtonCount == 5 && !LevelCompleted)
        {
            LevelCompleted = true;
            LevelComplete();
        }

        if(TimerRunning)
        {
            Timer += Time.deltaTime;
            DisplayTime(Timer);
        }

    }

    public void CountIncrease()
    {
        ButtonCount++;
    }

    public void LevelComplete()
    {
        LevelCompleteMenu.SetActive(true);
        TimerRunning = false;
        FinalGameTime.text = "Your Time: " + FormatTime(Timer);

    }

    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void DisplayTime(float TimeToDisplay)
    {
        float minutes = Mathf.FloorToInt(TimeToDisplay / 60);
        float seconds = Mathf.FloorToInt(TimeToDisplay % 60);
        if (TimerText != null)
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
