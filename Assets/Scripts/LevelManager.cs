using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    private double globalTimer = 0;
    private int globalTimerCheck = 1;
    private int upgradeTimer = 10;
    private System.Random RNG = new System.Random();

    private List<string> obstacleTypesInScene = new List<string>();

    [SerializeField] private LevelBuilder levelBuilder;

    public event EventHandler OneSecondHasPassed;


    private void Start()
    {
        levelBuilder.Build();

        // Populates a list (obstacleTypesInScene) with the names of each obstacle type currently in the scene
        for (int i = 0; i < levelBuilder.tileMap.GetLength(1); i++)
            for (int j = 0; j < levelBuilder.tileMap.GetLength(2); j++)
                // Skips all obstacles (tiles on the obstacle layer) that are not assigned
                if (levelBuilder.tileMap[1, i, j].name != null)
                    // Checks obstacle names and adds said name if it is defined in the switch statment and has yet to be added to the list (obstacleTypesInScene), then adds that name to the list (obstacleTypesInScene)
                    switch (levelBuilder.tileMap[1, i, j].name)
                    {
                        case var _ when levelBuilder.tileMap[1, i, j].name.Contains("Spikes") && !obstacleTypesInScene.Contains("Spikes"):
                                obstacleTypesInScene.Add("Spikes");
                            break;

                        case var _ when levelBuilder.tileMap[1, i, j].name.Contains("Shooter") && !obstacleTypesInScene.Contains("Shooter"):
                                obstacleTypesInScene.Add("Shooter");
                            break;
                    }
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
            OnOneSecondHasPassed();
            if (upgradeTimer <= 0)
            {
                TenSecondEffect();
                upgradeTimer = 10;
            }
        }
    }

    // Is called after every ten seconds, Upgrades obstucles and changes visual effects (IN PROGRESS)
    private void TenSecondEffect()
    {
        if (obstacleTypesInScene.Count != 0)
        {
            RemoveMaxLevelObstacles();
            UpgradeRandomObstacle();
        }
    }

    protected virtual void OnOneSecondHasPassed()
    {
        
        if (OneSecondHasPassed != null)
            OneSecondHasPassed(this, null);
    }

    public void SubscribeToOneSecondHasPassed(Obstacle script)
    {
        OneSecondHasPassed += script.OnOneSecondHasPassed;
    }

    // Randomly upgrades a obstacle type
    private void UpgradeRandomObstacle()
    {
        // Generates the index number for the list (obstacleTypesInScene) to decide what to upgrade
        int indexToUpgrade = RNG.Next(0, obstacleTypesInScene.Count);
        // Finds each obstacle in scene of that type (name) and Upgrades it (calls the Upgrade Method)
        for (int i = 0; i < levelBuilder.tileMap.GetLength(1); i++)
            for (int j = 0; j < levelBuilder.tileMap.GetLength(2); j++)
                if (levelBuilder.tileMap[1, i, j].name != null)
                    if (levelBuilder.tileMap[1, i, j].name.Contains(obstacleTypesInScene[indexToUpgrade]))
                        levelBuilder.tileMap[1, i, j].gameObject.GetComponent<Obstacle>().Upgrade();
    }

    // Removes all entries in the list (obstacleTypesInScene) that have reached max level aka can no longer be upgraded
    private void RemoveMaxLevelObstacles()
    {
        // Goes through all current entries in the list (obstacleTypesInScene)
        for (int k = 0; k < obstacleTypesInScene.Count; k++)
        {
            string obstacleName = obstacleTypesInScene[k];

            // Finds a obstacle of same type (name) as obstacleName and checks if its reached max level, if it has, remove from list (obstacleTypesInScene)
            for (int i = 0; i < levelBuilder.tileMap.GetLength(1); i++)
                for (int j = 0; j < levelBuilder.tileMap.GetLength(2); j++)
                    if (levelBuilder.tileMap[1, i, j].name != null)
                        if (levelBuilder.tileMap[1, i, j].name.Contains(obstacleName))
                        {
                            // Calls the Method in Obstacle (Class) to get true or false
                            if (levelBuilder.tileMap[1, i, j].gameObject.GetComponent<Obstacle>().IsObstacleAtMaxLevel())
                            {
                                obstacleTypesInScene.Remove(obstacleName);
                                // Moves pointer for the lists (obstacleTypesInScene) index down to make sure no obstacle type is missed
                                k--;
                            }
                            // Takes us out of the double for loop once a obstacle of the same type (name) is fonud because we only need to find 1 to check
                            i = levelBuilder.tileMap.GetLength(1); j = levelBuilder.tileMap.GetLength(2);
                        }
        }
    }
}
