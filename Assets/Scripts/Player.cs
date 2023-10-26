using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private float acceleration = 2;
    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float deceleration = 1;
    [SerializeField] private float cameraHeight = 5;

    public bool canMove;

    public Rigidbody rb;

    private bool upPressed = false;
    private bool downPressed = false;
    private bool rightPressed = false;
    private bool leftPressed = false;
    public bool pausePressed = false;

    //anything that hits the player sets this to true, Program Manager then takes this info to handles the lose state
    public bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        camera.transform.position = new Vector3(transform.position.x, cameraHeight, transform.position.z);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update camera to follow player
        camera.transform.position = new Vector3(transform.position.x, cameraHeight, transform.position.z);

        if (canMove)
        {
            //grabs player input and stores it
            if (Input.GetKey(KeyCode.W))
                upPressed = true;

            if (Input.GetKey(KeyCode.S))
                downPressed = true;

            if (Input.GetKey(KeyCode.D))
                rightPressed = true;

            if (Input.GetKey(KeyCode.A))
                leftPressed = true;
        }

        //used in Program Manager
        if (Input.GetKey(KeyCode.P))
            pausePressed = true;
    }

    private void FixedUpdate()
    {
        

        //Force Adding Variables to zero for the next add force
        float moveForwardBackward = 0;
        float moveLeftRight = 0;

        //Turns WASD Input into the Add Force Variables 
        if (upPressed)
        {
            moveForwardBackward += 1;
            upPressed = false;
        }
        if (downPressed)
        {
            moveForwardBackward -= 1;
            downPressed = false;
        }
        if (rightPressed)
        {
            moveLeftRight += 1;
            rightPressed = false;
        }
        if (leftPressed)
        {
            moveLeftRight -= 1;
            leftPressed = false;
        }

        //Uses the Add force Variables to Add force as well as capping the speed
        if (Math.Sqrt(Math.Pow(rb.velocity.x, 2) + Math.Pow(rb.velocity.z, 2)) < maxSpeed)
            rb.AddForce(new Vector3(moveLeftRight, 0, moveForwardBackward).normalized * Time.deltaTime * acceleration * 1000);

        //Increases deceleration to prevent sliding
        rb.velocity = new Vector3(rb.velocity.x * (1 - (0.1f * deceleration)), rb.velocity.y, rb.velocity.z * (1 - (0.1f * deceleration)));
    }
}
