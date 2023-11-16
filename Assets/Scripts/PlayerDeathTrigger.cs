using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathTrigger : MonoBehaviour
{
    private bool startupOver = false;

    private void Start()
    {
        startupOver = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && startupOver)
        {
            // Setting isHit to true causes a lose state
            other.GetComponent<Player>().isHit = true;
        }
    }
}
