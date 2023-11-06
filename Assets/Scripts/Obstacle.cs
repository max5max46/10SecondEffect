using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected string name;
    [SerializeField] protected int levelMax;
    protected int level = 1;

    public virtual void OnOneSecondHasPassed(object source, EventArgs e)
    {

    }

    public virtual void Upgrade()
    {

    }

    public int GetLevel()
    {
        return level;
    }

    public bool IsObstacleAtMaxLevel()
    {
        if (level < levelMax)
            return false;
        else
            return true;
    }
}
