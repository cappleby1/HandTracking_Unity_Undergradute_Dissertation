using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject LevelSelectMenu;

    public void start()
    {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        LevelSelectMenu.SetActive(false);
    }

    // Used to exit the game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Exiting");
    }

    //Display options menu
    public void DisplayOptionsMenu()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    // Display main menu from options menu
    public void OptionsMenuBack()
    {
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    // Display level select menu
    public void DisplayLevelSelect()
    {
        MainMenu.SetActive(false);
        LevelSelectMenu.SetActive(true);
    }

    // Used to open selected level
    public void LevelSelector(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }
}
