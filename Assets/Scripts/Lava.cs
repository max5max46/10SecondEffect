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

    private int waitTime = 5;
    private int currentWaitTime;

    private ApproachingSide approachingSide;
    private Vector3 lavaDefault;
    private float lerpTarget;
    private float perivousDistanceBetweenLavaPools;
    private float distanceBetweenLavaPools;
    private float distanceIntoMap = Global.LAVA_DEFAULT_DISTANCE_INTO_MAP;

    // Passed in by levelBuilder
    [HideInInspector] public int mapWidth;
    [HideInInspector] public int mapDepth;
    [HideInInspector] public float scale;

    // Start is called before the first frame update
    void Start()
    {
        name = Global.LAVA_NAME;

        //Sets scale for both lava pools
        lavaL.transform.localScale = new Vector3(scale * mapWidth, scale, scale * mapDepth);
        lavaR.transform.localScale = new Vector3(scale * mapWidth, scale, scale * mapDepth);

        distanceBetweenLavaPools = mapWidth * 2 * scale;
        perivousDistanceBetweenLavaPools = distanceBetweenLavaPools;
        currentWaitTime = waitTime;
        approachingSide = ApproachingSide.Left;

        lavaDefault = new Vector3 ((lavaL.transform.position.x - mapWidth) * scale, lavaL.transform.position.y + scale, lavaL.transform.position.z);

        lerpTarget = lavaDefault.x * distanceIntoMap;

        lavaL.transform.position = lavaDefault;
        lavaR.transform.position = new Vector3 (lavaDefault.x + distanceBetweenLavaPools, lavaDefault.y, lavaDefault.z);
    }

    // Update is called once per frame
    void Update()
    {
        //CHANGE LATER
        lavaL.transform.position = new Vector3(Mathf.Lerp(lavaL.transform.position.x, lerpTarget, Time.deltaTime * 1), lavaDefault.y, lavaDefault.z);
        lavaR.transform.position = new Vector3(lavaL.transform.position.x + Mathf.Lerp(perivousDistanceBetweenLavaPools, distanceBetweenLavaPools, Time.deltaTime * 1), lavaDefault.y, lavaDefault.z);
    }
    public override void ObstacleUpdate()
    {
        if (currentWaitTime <= 0)
        {
            currentWaitTime = waitTime;

            if (approachingSide == ApproachingSide.Left)
            {
                approachingSide = ApproachingSide.Right;
                lerpTarget = lavaDefault.x;
            }
            else
            {
                approachingSide = ApproachingSide.Left;
                lerpTarget = lavaDefault.x * distanceIntoMap;
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
                waitTime--;
                break;

            case 3:
                waitTime--;
                break;

            default:

                break;
        }

    }
}
