using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Script References")]
    public SaveFileManager saveFileManager;

    [Header("Button References")]
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject level4;
    public GameObject level5;
    public GameObject level6;
    public GameObject level7;
    public GameObject level8;
    public GameObject level9;
    public GameObject level10;

    [HideInInspector] public int idToLoad;
    private List<GameObject> levelButtons = new List<GameObject>();

    void Start()
    {
        levelButtons.Add(level1);
        levelButtons.Add(level2);
        levelButtons.Add(level3);
        levelButtons.Add(level4);
        levelButtons.Add(level5);
        levelButtons.Add(level6);
        levelButtons.Add(level7);
        levelButtons.Add(level8);
        levelButtons.Add(level9);
        levelButtons.Add(level10);

        foreach (GameObject levelButton in levelButtons)
            levelButton.GetComponent<Button>().interactable = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!saveFileManager.CheckIfLevelUnlocked(0))
            saveFileManager.UnlockLevel(0);

        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (saveFileManager.CheckIfLevelUnlocked(i))
                levelButtons[i].GetComponent<Button>().interactable = true;
            else
                levelButtons[i].GetComponent<Button>().interactable = false;
        }
    }

    public void SetLoadId(int id)
    {
        idToLoad = id;
    }
}
