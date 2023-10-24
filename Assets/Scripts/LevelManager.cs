using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private double globalTimer = 0;
    private int globalTimerCheck = 1;

    public event EventHandler OneSecondHasPassed;

    // Update is called once per frame
    void Update()
    {
        globalTimer += Time.deltaTime;
        if (globalTimer >= globalTimerCheck) 
        {
            globalTimerCheck++;
            OnOneSecondHasPassed();
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
