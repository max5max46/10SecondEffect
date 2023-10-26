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

        // Checks if player has been hit by a hazard
        if (player.isHit)
            PlayerHit();
    }

    private void PausePressed()
    {
        // Swaps to pause state
        player.pausePressed = false;
        // Checks if the player is able to pause
        if (menuManager.CheckState() == 0)
        {
            PauseGame();
            menuManager.ChangeMenu(1);
        }
    }

    private void PlayerHit()
    {
        // Swaps to lose state
        player.isHit = false;
        PauseGame();
        menuManager.ChangeMenu(2);
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
}