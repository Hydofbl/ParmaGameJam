using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void TestGamePlay()
    {
        SceneManager.LoadScene(1);
    }

    public void ObserverMap()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
