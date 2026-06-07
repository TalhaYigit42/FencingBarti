using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
