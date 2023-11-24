using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    // Level Manager Settings
    public const int TIME_TO_UPGRADE = 10;

    // Obstacle Names
    public const string SPIKES_NAME = "Spikes";
    public const string SHOOTER_NAME = "Shooter";
    public const string LAVA_NAME = "Lava";

    // Obstacle Max Level
    public const int SPIKES_LEVEL_MAX = 3;
    public const int SHOOTER_LEVEL_MAX = 5;
    public const int LAVA_LEVEL_MAX = 1;

    // Shooter Settings
    public const int SHOOTER_DEFAULT_WAIT_TIME = 2;
    public const float SHOOTER_BASE_SHOT_SPEED = 0.5f;

    // Spike Settings
    public const int SPIKES_DEFAULT_WAIT_TIME = 1;
    public const float SPIKES_HALF_OUT_HEIGHT = 0.5f;
    public const float SPIKES_FULL_OUT_HEIGHT = 1.3f;
    public const float SPIKE_LAUNCH_SPEED = 17;

    // Lava Settings
    public const int LAVA_DEFAULT_WAIT_TIME = 6;
    public const float LAVA_DEFAULT_DISTANCE_INTO_MAP = 0.9f;
}
