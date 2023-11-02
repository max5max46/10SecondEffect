using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    protected int level = 1;
    protected int levelMax;

    public virtual void OnOneSecondHasPassed(object source, EventArgs e)
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
