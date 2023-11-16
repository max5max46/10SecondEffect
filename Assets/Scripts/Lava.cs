using System.Collections;
using System.Collections.Generic;
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
    private int currentWaitTime;

    private ApproachingSide approachingSide;
    private Vector3 lavaDefault;
    private Vector3 lerpTarget;
    private float distanceBetweenPools;
    private float distanceBetweenPoolsModifier = Global.LAVA_DEFAULT_DISTANCE_BETWEEN_POOLS_MODIFIER;
    private float perDistanceBetweenPoolsModifier = Global.LAVA_DEFAULT_DISTANCE_BETWEEN_POOLS_MODIFIER;
    private float distanceIntoMap = Global.LAVA_DEFAULT_DISTANCE_INTO_MAP;
    private float wallInsetOffset = 0.1f;

    private Vector3 velocity = Vector3.zero;

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

        distanceBetweenPools = (mapWidth + wallInsetOffset) * 2 * scale;
        currentWaitTime = waitTime;
        approachingSide = ApproachingSide.Left;

        lavaDefault = new Vector3 ((lavaL.transform.position.x - (mapWidth + wallInsetOffset)) * scale, lavaL.transform.position.y + scale, lavaL.transform.position.z);

        lerpTarget = new Vector3 (lavaDefault.x * distanceIntoMap, lavaDefault.y, lavaDefault.z);

        lavaL.transform.position = lavaDefault;
        lavaR.transform.position = new Vector3 (lavaDefault.x + distanceBetweenPools, lavaDefault.y, lavaDefault.z);
    }

    // Update is called once per frame
    void Update()
    {
        //CHANGE LATER
        //lavaL.transform.position = new Vector3(Mathf.Lerp(lavaL.transform.position.x, lerpTarget, Time.deltaTime * 1), lavaDefault.y, lavaDefault.z);
        lavaL.transform.position = Vector3.SmoothDamp(lavaL.transform.position, lerpTarget, ref velocity, waitTime/2);
        lavaR.transform.position = new Vector3(lavaL.transform.position.x + Mathf.Lerp(lavaR.transform.position.x, distanceBetweenPools * distanceBetweenPoolsModifier, Time.deltaTime * 0.1f), lavaDefault.y, lavaDefault.z);
    }
    public override void ObstacleUpdate()
    {
        if (currentWaitTime <= 0)
        {
            currentWaitTime = waitTime;

            if (approachingSide == ApproachingSide.Left)
            {
                approachingSide = ApproachingSide.Right;
                lerpTarget = lavaDefault;
            }
            else
            {
                approachingSide = ApproachingSide.Left;
                lerpTarget = new Vector3 (lavaDefault.x * distanceIntoMap, lavaDefault.y, lavaDefault.z);
            }
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
                distanceIntoMap = 0.8f;
                break;

            case 3:
                perDistanceBetweenPoolsModifier = distanceBetweenPools;
                distanceBetweenPools = 0.85f;
                break;

            case 4:
                waitTime--;
                break;

            case 5:
                distanceIntoMap = 0.65f;
                break;

            case 6:
                perDistanceBetweenPoolsModifier = distanceBetweenPools;
                distanceBetweenPools = 0.7f;
                break;

            case 7:
                waitTime--;
                break;

            case 8:
                distanceIntoMap = 0.5f;
                break;

            case 9:
                perDistanceBetweenPoolsModifier = distanceBetweenPools;
                distanceBetweenPools = 0.5f;
                break;

            case 10:
                waitTime--;
                break;

            default:
                
                break;
        }

    }
}
