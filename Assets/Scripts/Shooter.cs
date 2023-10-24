using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Shooter : Obstacle
{
    [SerializeField] private GameObject bulletPrefab;

    enum ShooterState
    {
        Wait,
        AboutToShoot,
        Shoot
    }

    private ShooterState state = ShooterState.Wait;
    private int waitTime = 2;
    private int currentWaitTime = 2;
    private float shotSpeed = 1.0f;

    public override void OnOneSecondHasPassed(object source, EventArgs e) 
    {
        Debug.Log(state);
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
                //Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

                bullet.GetComponent<Bullet>().speed = shotSpeed;
                //scales bullet according to shooter scale
                bullet.transform.localScale = new Vector3(transform.localScale.x * bullet.transform.localScale.x, transform.localScale.y * bullet.transform.localScale.y, transform.localScale.z * bullet.transform.localScale.z);
                
                currentWaitTime = waitTime;
                state = ShooterState.Wait;
                break;

        }
    }
}
