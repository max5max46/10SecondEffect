using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Structs;

public class Lava : Obstacle
{
    [Header("Game Object References")]
    [SerializeField] private GameObject lavaL;
    [SerializeField] private GameObject lavaR;

    enum ApproachingSide
    {
        Left,
        Right
    }

    private int waitTime = Global.LAVA_DEFAULT_WAIT_TIME;
    private int queuedUpgradeWaitTime;
    private int currentWaitTime;

    private ApproachingSide approachingSide;
    private Vector3 lavaDefaultL;
    private Vector3 lavaDefaultR;
    private float distanceIntoMap = Global.LAVA_DEFAULT_DISTANCE_INTO_MAP;
    private float queuedUpgradeDistanceIntoMap;
    private float wallInsetOffset = 0.1f;

    // Passed in by levelBuilder
    [HideInInspector] public int mapWidth;
    [HideInInspector] public int mapDepth;
    [HideInInspector] public float scale;

    // Start is called before the first frame update
    void Start()
    {
        name = Global.LAVA_NAME;

        // Accounts for outer walls
        mapWidth -= 2;

        // Sets scale for both lava pools
        lavaL.transform.localScale = new Vector3(scale * mapWidth, scale, scale * mapDepth);
        lavaR.transform.localScale = new Vector3(scale * mapWidth, scale, scale * mapDepth);

        // Adds leeway between the model and collider to not feel unfair to the player
        lavaL.GetComponent<BoxCollider>().size = new Vector3((lavaL.transform.localScale.x - 0.5f) / lavaL.transform.localScale.x, 1, 1);
        lavaR.GetComponent<BoxCollider>().size = new Vector3((lavaR.transform.localScale.x - 0.5f) / lavaR.transform.localScale.x, 1, 1);

        currentWaitTime = waitTime;
        approachingSide = ApproachingSide.Left;
        queuedUpgradeWaitTime = waitTime;
        queuedUpgradeDistanceIntoMap = distanceIntoMap;

        // Sets the default position of each lava pool, the default is considered the position that keeps the lava outside the level on either side
        lavaDefaultL = new Vector3 ((-(mapWidth + wallInsetOffset)) * scale, scale, 0);
        lavaDefaultR = new Vector3 ((mapWidth + wallInsetOffset) * scale, scale, 0);

        // Sets lava pools position to the defaults
        lavaL.transform.position = lavaDefaultL;
        lavaR.transform.position = lavaDefaultR;
    }

    // Update is called once per frame
    void Update()
    {

        float diffenceR = lavaDefaultR.x - (lavaDefaultR.x * distanceIntoMap);
        float diffenceL = lavaDefaultL.x - (lavaDefaultL.x * distanceIntoMap);

        // Moves the left lava deeper into the level and the right lava out of the level at a pace realative to the waitTime
        if (approachingSide == ApproachingSide.Left)
        {
            if (lavaL.transform.position.x < lavaDefaultL.x * distanceIntoMap)
                lavaL.transform.position = new Vector3(lavaL.transform.position.x + ((-diffenceL / (waitTime + 1)) * Time.deltaTime), lavaDefaultL.y, lavaDefaultL.z);
            // Clamps the position so it never overshoots its target
            if (lavaL.transform.position.x > lavaDefaultL.x * distanceIntoMap)
                lavaL.transform.position = new Vector3(lavaDefaultL.x * distanceIntoMap, lavaDefaultL.y, lavaDefaultL.z);

            if (lavaR.transform.position.x < lavaDefaultR.x)
                lavaR.transform.position = new Vector3(lavaR.transform.position.x + ((diffenceR / (waitTime + 1)) * Time.deltaTime), lavaDefaultR.y, lavaDefaultR.z);
            // Clamps the position so it never overshoots its target
            if (lavaR.transform.position.x > lavaDefaultR.x)
                lavaR.transform.position = lavaDefaultR;
        }

        // Moves the right lava deeper into the level and the left lava out of the level at a pace realative to the waitTime
        if (approachingSide == ApproachingSide.Right)
        {
            if (lavaL.transform.position.x > lavaDefaultL.x)
                lavaL.transform.position = new Vector3(lavaL.transform.position.x + ((diffenceL / (waitTime + 1)) * Time.deltaTime), lavaDefaultL.y, lavaDefaultL.z);
            // Clamps the position so it never overshoots its target
            if (lavaL.transform.position.x < lavaDefaultL.x)
                lavaL.transform.position = lavaDefaultL;

            if (lavaR.transform.position.x > lavaDefaultR.x * distanceIntoMap)
                lavaR.transform.position = new Vector3(lavaR.transform.position.x + ((-diffenceR / (waitTime + 1)) * Time.deltaTime), lavaDefaultR.y, lavaDefaultR.z);
            // Clamps the position so it never overshoots its target
            if (lavaR.transform.position.x < lavaDefaultR.x * distanceIntoMap)
                lavaR.transform.position = new Vector3(lavaDefaultR.x * distanceIntoMap, lavaDefaultR.y, lavaDefaultR.z);
        }

    }

    public override void ObstacleUpdate()
    {
        if (currentWaitTime <= 0)
        {
            distanceIntoMap = queuedUpgradeDistanceIntoMap;
            waitTime = queuedUpgradeWaitTime;

            currentWaitTime = waitTime;

            if (approachingSide == ApproachingSide.Left)
                approachingSide = ApproachingSide.Right;
            else
                approachingSide = ApproachingSide.Left;
        }
        else
        {
            currentWaitTime--;
        }
    }

    public override void Upgrade()
    {
        Debug.Log("lava Upgraded");

        level++;

        switch (level)
        {
            case 2:
                queuedUpgradeDistanceIntoMap = 0.85f;
                break;

            case 3:
                queuedUpgradeDistanceIntoMap = 0.8f;
                break;

            case 4:
                queuedUpgradeWaitTime = waitTime--;
                break;

            case 5:
                queuedUpgradeDistanceIntoMap = 0.75f;
                break;

            case 6:
                queuedUpgradeDistanceIntoMap = 0.65f;
                break;

            case 7:
                queuedUpgradeWaitTime = waitTime--;
                break;

            case 8:
                queuedUpgradeDistanceIntoMap = 0.6f;
                break;

            case 9:
                queuedUpgradeDistanceIntoMap = 0.5f;
                break;

            case 10:
                queuedUpgradeWaitTime = waitTime--;
                break;

            default:
                
                break;
        }

    }
}
