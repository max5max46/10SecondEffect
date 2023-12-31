using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    private double globalTimer = 0;
    private int globalTimerCheck = 1;
    private int upgradeTimer = Global.TIME_TO_UPGRADE;
    private System.Random RNG = new System.Random();

    [SerializeField] private LevelBuilder levelBuilder;

    private UIManager uiManager;
    private MenuManager menuManager;
    private SaveFileManager saveFileManager;
    private LevelSelectManager levelSelectManager;
    private Player player;
    private LevelData levelData;

    private void Start()
    {
        levelSelectManager = FindAnyObjectByType<LevelSelectManager>();

        levelData = levelBuilder.LoadAndBuildLevel(levelSelectManager.idToLoad);

        uiManager = FindAnyObjectByType<UIManager>();
        menuManager = FindAnyObjectByType<MenuManager>();
        saveFileManager = FindAnyObjectByType<SaveFileManager>();
        player = FindAnyObjectByType<Player>();

        uiManager.ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        globalTimer += Time.deltaTime;

        // Checks if player has been hit by a hazard, switchs to lose state
        if (player.isHit)
            PlayerLoses();

        // Checks if a second has passed
        if (globalTimer >= globalTimerCheck) 
        {
            globalTimerCheck++;
            upgradeTimer--;

            Debug.Log(upgradeTimer);

            ObstacleUpdate();
            UpdateUI();

            // Checks if 10 seconds has passed
            if (upgradeTimer <= 0)
            {
                TenSecondEffect();
                upgradeTimer = Global.TIME_TO_UPGRADE;
            }
        }
    }

    // Is called after every ten seconds, Upgrades obstacles and checks for win state
    private void TenSecondEffect()
    {

        RemoveMaxLevelObstacles();

        // Checks if all Obstacles have hit max level, if true, Win the level
        if (levelData.obstacleTypesInLevel.Count != 0)
            UpgradeRandomObstacle();
        else
            PlayerWins();

    }

    // Tells all Obstacles to update, this occurs by default every scond
    protected virtual void ObstacleUpdate()
    {
        // For tile map obstacles
        for (int i = 0; i < levelData.tileMap.GetLength(1); i++)
            for (int j = 0; j < levelData.tileMap.GetLength(2); j++)
                if (levelData.tileMap[1, i, j].gameObject != null)
                    levelData.tileMap[1, i, j].gameObject.GetComponent<Obstacle>().ObstacleUpdate();

        // For non tile map obstacles
        if (levelData.isLavaOn)
            levelData.lavaGameObject.GetComponent<Obstacle>().ObstacleUpdate();
    }

    // Updates the UI in uiManager
    private void UpdateUI()
    {
        uiManager.timer++;
    }

    // Randomly upgrades a obstacle type
    private void UpgradeRandomObstacle()
    {

        // Generates the index number for the list (levelData.obstacleTypesInLevel) to decide what to upgrade
        int indexToUpgrade = RNG.Next(0, levelData.obstacleTypesInLevel.Count);

        // For tile map obstacles
        // Finds each obstacle in scene of that type (name) and Upgrades it (calls the Upgrade Method)
        for (int i = 0; i < levelData.tileMap.GetLength(1); i++)
            for (int j = 0; j < levelData.tileMap.GetLength(2); j++)
                if (levelData.tileMap[1, i, j].name != null)
                    if (levelData.tileMap[1, i, j].name == levelData.obstacleTypesInLevel[indexToUpgrade])
                        levelData.tileMap[1, i, j].gameObject.GetComponent<Obstacle>().Upgrade();

        // For non tile map obstacles
        if (levelData.isLavaOn && levelData.obstacleTypesInLevel[indexToUpgrade] == Global.LAVA_NAME)
            levelData.lavaGameObject.GetComponent<Obstacle>().Upgrade();
    }

    // Removes all entries in the list (levelData.obstacleTypesInLevel) that have reached max level aka can no longer be upgraded
    private void RemoveMaxLevelObstacles()
    {
        List<string> obstacleTypesInLevelTemp = new List<string>(levelData.obstacleTypesInLevel);

        // Goes through all current entries in the list (levelData.obstacleTypesInLevel)
        foreach (string obstacleName in levelData.obstacleTypesInLevel)
        {
            // For tile map obstacles
            // Finds a obstacle of same type (name) as obstacleName and checks if its reached max level, if it has, remove from list (obstacleTypesInLevelTemp)
            for (int y = 0; y < levelData.tileMap.GetLength(1); y++)
                for (int x = 0; x < levelData.tileMap.GetLength(2); x++)
                    if (levelData.tileMap[1, y, x].name != null)
                        if (levelData.tileMap[1, y, x].name == obstacleName)
                        {
                            // Calls the Method in Obstacle (Class) to get true or false
                            if (levelData.tileMap[1, y, x].gameObject.GetComponent<Obstacle>().IsObstacleAtMaxLevel())
                                obstacleTypesInLevelTemp.Remove(obstacleName);

                            // Takes us out of the double for loop once a obstacle of the same type (name) is found because we only need to find 1 to check
                            y = levelData.tileMap.GetLength(1); x = levelData.tileMap.GetLength(2);
                        }

            // For non tile map obstacles
            if (Global.LAVA_NAME == obstacleName)
                if (levelData.lavaGameObject.GetComponent<Obstacle>().IsObstacleAtMaxLevel())
                    obstacleTypesInLevelTemp.Remove(obstacleName);

        }

        levelData.obstacleTypesInLevel = obstacleTypesInLevelTemp;
    }

    private void PlayerLoses()
    {
        // Swaps to lose state
        player.isHit = false;
        player.rb.velocity = Vector3.zero;
        ProgramManager.PauseGame();
        menuManager.ChangeMenu(5);
    }

    private void PlayerWins()
    {
        saveFileManager.UnlockLevel(levelData.id + 1);
        player.rb.velocity = Vector3.zero;
        ProgramManager.PauseGame();
        menuManager.ChangeMenu(6);
    }

}
