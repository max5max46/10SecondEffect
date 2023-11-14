using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    protected string name;
    protected int levelMax { get; set; }
    protected int level { get; set; }

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
