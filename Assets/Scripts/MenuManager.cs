using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public enum MenuState
    {
        Gameplay,
        Title,
        Options,
        LevelSelect,
        Pause,
        Lose,
        Win
    }

    private MenuState state;

    public GameObject gameplayUI;
    public GameObject titleScreen;
    public GameObject optionsMenu;
    public GameObject levelSelect;
    public GameObject pauseMenu;
    public GameObject loseScreen;
    public GameObject winScreen;


    // Start is called before the first frame update
    void Start()
    {
        ChangeMenu((int)MenuState.Title);
    }

    // Changes Menu according to the index of the enum MenuState
    public void ChangeMenu(int state)
    {
        this.state = (MenuState)state;

        gameplayUI.SetActive(false);
        titleScreen.SetActive(false);
        optionsMenu.SetActive(false);
        levelSelect.SetActive(false);
        pauseMenu.SetActive(false);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        switch (this.state)
        {
            case MenuState.Gameplay:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gameplayUI.SetActive(true);
                break;

            case MenuState.Title:
                titleScreen.SetActive(true);
                break;

            case MenuState.Options:
                optionsMenu.SetActive(true);
                break;

            case MenuState.LevelSelect:
                levelSelect.SetActive(true);
                break;

            case MenuState.Pause:
                pauseMenu.SetActive(true);
                break;

            case MenuState.Lose:
                loseScreen.SetActive(true);
                break;

            case MenuState.Win:
                winScreen.SetActive(true);
                break;
        }
    }

    public int CheckState()
    {
        return (int)state;
    }
}
