using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    // Obstacle Names
    public const string SPIKES_NAME = "Spikes";
    public const string SHOOTER_NAME = "Shooter";
    public const string LAVA_NAME = "Lava";

    // Obstacle Max Level
    public const int SPIKES_LEVEL_MAX = 3;
    public const int SHOOTER_LEVEL_MAX = 5;
    public const int LAVA_LEVEL_MAX = 1;

    // Shooter Settings
    public const float SHOOTER_BASE_SHOT_SPEED = 0.5f;

    // Spike Settings
    public const float SPIKES_HALF_OUT_HEIGHT = 0.5f;
    public const float SPIKES_FULL_OUT_HEIGHT = 1.3f;
    public const float SPIKE_LAUNCH_SPEED = 17;
}
