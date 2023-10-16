using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    enum ShooterState
    {
        Wait,
        AboutToShoot,
        Shoot
    }

    private ShooterState state = ShooterState.Wait;
    private int waitTime = 3;
    private int currentWaitTime = 3;
    private float shotSpeed = 1.0f;

    public void OnOneSecondHasPassed(object source, EventArgs e) 
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
                bullet.GetComponent<Bullet>().speed = shotSpeed;
                Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z);
                Instantiate(bullet, transform.position, transform.rotation);
                currentWaitTime = waitTime;
                state = ShooterState.Wait;
                break;

        }
    }
}
