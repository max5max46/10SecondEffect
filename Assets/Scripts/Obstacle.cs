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

    public bool IsObstacleAtMaxLevel()
    {
        if (level < levelMax)
            return false;
        else
            return true;
    }
}
