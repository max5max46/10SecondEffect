using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public enum MenuState
    {
        None,
        Pause,
        Lose
    }

    private MenuState state;

    public GameObject pauseMenu;
    public GameObject loseScreen;


    // Start is called before the first frame update
    void Start()
    {
        ChangeMenu((int)MenuState.None);
    }

    // Changes Menu according to the index of the enum MenuState
    public void ChangeMenu(int state)
    {
        this.state = (MenuState)state;

        pauseMenu.SetActive(false);
        loseScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        switch (this.state)
        {
            case MenuState.None:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case MenuState.Pause:
                pauseMenu.SetActive(true);
                break;

            case MenuState.Lose:
                loseScreen.SetActive(true);
                break;
        }
    }

    public int CheckState()
    {
        return (int)state;
    }
}
