using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.isGamePaused)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Time.timeScale = 0f;
                GameManager.isGamePaused = true;
                pauseMenu.SetActive(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
                GameManager.isGamePaused = false;
                pauseMenu.SetActive(false);
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
