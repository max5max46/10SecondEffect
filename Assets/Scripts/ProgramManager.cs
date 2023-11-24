using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgramManager : MonoBehaviour
{

    [SerializeField] MenuManager menuManager;
    [SerializeField] Player player;

    private void Update()
    {
        // Checks if player hit the pause button
        if (player.pausePressed)
            PausePressed();
    }

    private void PausePressed()
    {
        // Swaps to pause state
        player.pausePressed = false;
        // Checks if the player is able to pause
        if (menuManager.CheckState() == 0)
        {
            PauseGame();
            menuManager.ChangeMenu(4);
        }
    }

    public static void ResetScene()
    {
        // Resets scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void PauseGame()
    {
        // Pauses game (temp)
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        // Unpauses game (temp)
        Time.timeScale = 1;
    }

    public static void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void ExitApplication()
    {
        Application.Quit();
    }
}
