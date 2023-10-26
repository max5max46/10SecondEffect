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

    private List<string> obstacleTypesInScene;

    [SerializeField] private LevelBuilder levelBuilder;

    public event EventHandler OneSecondHasPassed;


    private void Start()
    {
        levelBuilder.Build();

        /*for (int i = 0; i < levelBuilder.loadedMap.GetLength(1); i++)
            for (int j = 0; j < levelBuilder.loadedMap.GetLength(2); j++)
                switch (levelBuilder.loadedMap[1,i,j]) 
                {
                    case 1:
                        if (!obstacleTypesInScene.Contains("spikes"))
                            obstacleTypesInScene.Add("spikes");
                        break;

                    case 2:
                        if (!obstacleTypesInScene.Contains("shooter"))
                            obstacleTypesInScene.Add("shooter");
                        break;
                }*/
    }

    // Update is called once per frame
    void Update()
    {
        globalTimer += Time.deltaTime;
        if (globalTimer >= globalTimerCheck) 
        {
            globalTimerCheck++;
            upgradeTimer--;
            OnOneSecondHasPassed();
            if (upgradeTimer <= 0)
            {
                upgradeTimer = 10;
            }
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
}
