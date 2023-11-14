using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    private double globalTimer = 0;
    private int globalTimerCheck = 1;
    private int upgradeTimer = 10;
    private System.Random RNG = new System.Random();

    [SerializeField] private LevelBuilder levelBuilder;

    private LevelData levelData;

    private void Start()
    {
        levelData = levelBuilder.LoadAndBuildLevel();
    }

    // Update is called once per frame
    void Update()
    {
        globalTimer += Time.deltaTime;
        if (globalTimer >= globalTimerCheck) 
        {
            globalTimerCheck++;
            upgradeTimer--;
            Debug.Log(upgradeTimer);
            ObstacleUpdate();
            if (upgradeTimer <= 0)
            {
                TenSecondEffect();
                upgradeTimer = 10;
            }
        }
    }

    // Is called after every "ten" seconds (CHANGE LATER), Upgrades obstucles and changes visual effects (IN PROGRESS)
    private void TenSecondEffect()
    {

        if (levelData.obstacleTypesInLevel.Count != 0)
            RemoveMaxLevelObstacles();
        if (levelData.obstacleTypesInLevel.Count != 0)
            UpgradeRandomObstacle();

        UpdateUI();
    }

    // Tells all Obstacles to update, this occurs by default every scond
    protected virtual void ObstacleUpdate()
    {
        for (int i = 0; i < levelData.tileMap.GetLength(1); i++)
            for (int j = 0; j < levelData.tileMap.GetLength(2); j++)
                if (levelData.tileMap[1, i, j].gameObject != null)
                    levelData.tileMap[1, i, j].gameObject.GetComponent<Obstacle>().ObstacleUpdate();
    }

    // Randomly upgrades a obstacle type
    private void UpgradeRandomObstacle()
    {
        // Generates the index number for the list (levelData.obstacleTypesInLevel) to decide what to upgrade
        int indexToUpgrade = RNG.Next(0, levelData.obstacleTypesInLevel.Count);
        // Finds each obstacle in scene of that type (name) and Upgrades it (calls the Upgrade Method)
        for (int i = 0; i < levelData.tileMap.GetLength(1); i++)
            for (int j = 0; j < levelData.tileMap.GetLength(2); j++)
                if (levelData.tileMap[1, i, j].name != null)
                    if (levelData.tileMap[1, i, j].name.Contains(levelData.obstacleTypesInLevel[indexToUpgrade]))
                        levelData.tileMap[1, i, j].gameObject.GetComponent<Obstacle>().Upgrade();
    }

    // Removes all entries in the list (levelData.obstacleTypesInLevel) that have reached max level aka can no longer be upgraded
    private void RemoveMaxLevelObstacles()
    {
        // Goes through all current entries in the list (levelData.obstacleTypesInLevel)
        for (int k = 0; k < levelData.obstacleTypesInLevel.Count; k++)
        {
            string obstacleName = levelData.obstacleTypesInLevel[k];

            // Finds a obstacle of same type (name) as obstacleName and checks if its reached max level, if it has, remove from list (levelData.obstacleTypesInLevel)
            for (int i = 0; i < levelData.tileMap.GetLength(1); i++)
                for (int j = 0; j < levelData.tileMap.GetLength(2); j++)
                    if (levelData.tileMap[1, i, j].name != null)
                        if (levelData.tileMap[1, i, j].name.Contains(obstacleName))
                        {
                            // Calls the Method in Obstacle (Class) to get true or false
                            if (levelData.tileMap[1, i, j].gameObject.GetComponent<Obstacle>().IsObstacleAtMaxLevel())
                            {
                                levelData.obstacleTypesInLevel.Remove(obstacleName);
                                // Moves pointer for the lists (levelData.obstacleTypesInLevel) index down to make sure no obstacle type is missed
                                k--;
                            }
                            // Takes us out of the double for loop once a obstacle of the same type (name) is fonud because we only need to find 1 to check
                            i = levelData.tileMap.GetLength(1); j = levelData.tileMap.GetLength(2);
                        }
        }
    }

    private void UpdateUI()
    {

    }
}
