using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private double globalTimer = 0;
    private int globalTimerCheck = 1;

    [SerializeField] private Shooter shooterScript;

    public event EventHandler OneSecondHasPassed;

    // Start is called before the first frame update
    void Start()
    {
        OneSecondHasPassed += shooterScript.OnOneSecondHasPassed;
    }

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
}
