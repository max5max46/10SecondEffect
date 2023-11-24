using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using System;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timeText;

    [HideInInspector]public double timer;

    private void Update()
    {
        if (timer < 9999)
            timeText.text = TimeSpan.FromSeconds(timer).ToString();
        else
            timeText.text = "Error - # to high";
    }

    public void ResetTimer()
    {
        timer = 0;
    }


}
