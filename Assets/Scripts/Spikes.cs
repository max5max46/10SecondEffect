using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Obstacle
{
    
    [SerializeField] private GameObject spikeHolder;
    [SerializeField] private GameObject spike;

    enum SpikeState
    {
        Wait,
        AboutToSpike,
        Spike
    }

    public bool altTiming = false;
    private SpikeState state = SpikeState.Wait;
    private int waitTime = 1;
    private int currentWaitTime = 1;
    private float lerpTarget;
    private Vector3 spikeDefault;

    private void Start()
    {
        levelMax = 3;

        if (altTiming)
            state = SpikeState.AboutToSpike;

        spikeDefault = spike.transform.position;
        lerpTarget = spike.transform.position.y;
    }
    private void Update()
    {
        spike.transform.position = new Vector3(spikeDefault.x, Mathf.Lerp(spike.transform.position.y, lerpTarget, Time.deltaTime * 17), spikeDefault.z);
    }

    public override void OnOneSecondHasPassed(object source, EventArgs e)
    {
        //Debug.Log(state);
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
                lerpTarget = spikeDefault.y + 0.5f;
                state = SpikeState.Spike;
                break;
            case SpikeState.Spike:
                lerpTarget = spikeDefault.y + 1.3f;
                currentWaitTime = waitTime;
                if (waitTime != -1)
                    state = SpikeState.Wait;
                else
                    state = SpikeState.AboutToSpike;
                break;

        }
    }

    public override void Upgrade()
    {
        Debug.Log("spikes Upgraded");
        switch (level) 
        {
            case 1:
                waitTime = 0;
                level++;
                break;

            case 2:
                waitTime = -1;
                level++;
                break;
        }   

    }

}
