using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    protected string name;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    protected int levelMax;
    protected int level;

    public virtual void ObstacleUpdate()
    {

    }

    public virtual void Upgrade()
    {

    }

    public void SetStartingLevel(int level)
    {
        this.level = level;
    }

    public void SetMaxLevel(int levelMax)
    {
        this.levelMax = levelMax;
    }

    public bool IsObstacleAtMaxLevel()
    {
        if (level < levelMax)
            return false;
        else
            return true;
    }
}
