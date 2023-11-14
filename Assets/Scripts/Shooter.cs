using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : Obstacle
{
    [Header("Game Object References")]
    [SerializeField] private GameObject bulletPrefab;

    enum ShooterState
    {
        Wait,
        AboutToShoot,
        Shoot
    }

    private ShooterState state = ShooterState.Wait;
    private float shotSpeed;
    //Wait time in Seconds
    private int waitTime = 2;
    private int currentWaitTime;

    private void Start()
    {
        name = Global.SHOOTER_NAME;
        shotSpeed = Global.SHOOTER_BASE_SHOT_SPEED;
        currentWaitTime = waitTime;
    }

    public override void ObstacleUpdate() 
    {
        switch (state) 
        {
            case ShooterState.Wait:
                if (currentWaitTime == 0)
                    state = ShooterState.AboutToShoot;
                else
                    currentWaitTime--;
                break;

            case ShooterState.AboutToShoot:
                state = ShooterState.Shoot;
                break;

            case ShooterState.Shoot:
                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                // Passes shotSpeed to bullet
                bullet.GetComponent<Bullet>().speed = shotSpeed;
                // Scales bullet according to shooter scale
                bullet.transform.localScale = new Vector3(transform.localScale.x * bullet.transform.localScale.x, transform.localScale.y * bullet.transform.localScale.y, transform.localScale.z * bullet.transform.localScale.z);
                // If waitTime becomes negative, skip the Wait state 
                if (waitTime >= 0)
                {
                    currentWaitTime = waitTime;
                    state = ShooterState.Wait;
                }
                else
                    state = ShooterState.AboutToShoot;
                break;

        }
    }

    public override void Upgrade()
    {
        Debug.Log("shooter Upgraded");

        level++;

        switch (level)
        {
            case 2:
                waitTime--;
                break;

            case 3:
                shotSpeed *= 1.5f;
                break;

            case 4:
                waitTime--;
                break;

            case 5:
                waitTime--;
                break;
        }
    }
}
