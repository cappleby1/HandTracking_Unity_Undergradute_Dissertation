using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject levelSelectMenu;
    public GameObject mainMenu;
    
    public void Start()
    {
        mainMenu.SetActive(true);
    }

    public void MainMenuActive()
    {
        mainMenu.SetActive(true);
        levelSelectMenu.SetActive(false);
    }
    
    public void Level1Selected()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LevelSelect()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
    }
}
