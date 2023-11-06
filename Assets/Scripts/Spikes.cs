using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Obstacle
{

    [SerializeField] private float spikeHalfOutHeight;
    [SerializeField] private float spikeFullOutHeight;

    [Header("Game Object References")]
    [SerializeField] private GameObject spikeHolder;
    [SerializeField] private GameObject spike;

    enum SpikeState
    {
        Wait,
        AboutToSpike,
        Spike
    }

    //Wait time in Seconds
    private int waitTime = 1;
    private int currentWaitTime;

    [HideInInspector] public bool altTiming = false;
    private SpikeState state = SpikeState.Wait;
    private float lerpTarget;
    private Vector3 spikeDefault;


    private void Start()
    {
        currentWaitTime = waitTime;

        if (altTiming)
            currentWaitTime += 2;

        spikeDefault = spike.transform.position;
        lerpTarget = spike.transform.position.y;
    }

    private void Update()
    {
        spike.transform.position = new Vector3(spikeDefault.x, Mathf.Lerp(spike.transform.position.y, lerpTarget, Time.deltaTime * 17), spikeDefault.z);
    }

    public override void OnOneSecondHasPassed(object source, EventArgs e)
    {
        switch (state)
        {
            case SpikeState.Wait:

                lerpTarget = spikeDefault.y;
                if (currentWaitTime == 0)
                    state = SpikeState.AboutToSpike;
                else
                    currentWaitTime--;
                break;

            case SpikeState.AboutToSpike:

                lerpTarget = spikeDefault.y + spikeHalfOutHeight;
                state = SpikeState.Spike;
                break;

            case SpikeState.Spike:

                lerpTarget = spikeDefault.y + spikeFullOutHeight;

                // If waitTime becomes negative, skip the Wait state
                if (waitTime >= 0)
                {
                    currentWaitTime = waitTime;
                    state = SpikeState.Wait;
                }
                else
                    state = SpikeState.AboutToSpike;
                break;

        }
    }

    public override void Upgrade()
    {
        Debug.Log("spikes Upgraded");

        level++;

        switch (level) 
        {
            case 2:
                waitTime--;
                break;

            case 3:
                waitTime--;
                break;
        }   

    }

}
