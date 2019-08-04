﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int maxJumps;
    public float jumpHeight;
    public float speed;
    public CharacterState myState;

    private int jump;
    private bool timercheck = false;
    private float rayLength = .3f;
    private float rayLengthDown = .52f;
    private Rigidbody body;

    //the states the player can be in
    public enum CharacterState
    {
        GROUNDED,
        JUMPING,
        WALLGRABRIGHT,
        WALLGRABLEFT
    }

    // Start is called before the first frame update, or on creation of the object
    void Start()
    {
        myState = CharacterState.GROUNDED;
        body = GetComponent<Rigidbody>();
    }

    //the timeout that happens after jumping from a wall
    IEnumerator WallJumpTimer()
    {
        yield return new WaitForSeconds(.2f);
        timercheck = false;
    }

    // Update is called once per frame and is used for physics updates
    void FixedUpdate()
    {
        Raycasts();
        switch (myState)
        {
            case CharacterState.JUMPING:
                {
                    JumpingMovement();
                    break;
                }
            case CharacterState.WALLGRABRIGHT:
                {
                    WallGrabMovement(true);
                    break;
                }
            case CharacterState.WALLGRABLEFT:
                {
                    WallGrabMovement(false);
                    break;
                }
            case CharacterState.GROUNDED:
                {
                    GroundedMovement();
                    break;
                }
        }
    }
    
    //somewhere to put the messing looking raycasts
    private void Raycasts()
    {
        Vector3 topRight = new Vector3(transform.position.x, transform.position.y + .45f, transform.position.z);
        Vector3 bottomRight = new Vector3(transform.position.x, transform.position.y - .45f, transform.position.z);
        Vector3 topLeft = new Vector3(transform.position.x, transform.position.y + .45f, transform.position.z);
        Vector3 bottomLeft = new Vector3(transform.position.x, transform.position.y - .45f, transform.position.z);

        Vector3 right = transform.TransformDirection(new Vector3(rayLength, 0f, 0f));
        Vector3 left = transform.TransformDirection(new Vector3(-rayLength, 0f, 0f));
        Vector3 down = transform.TransformDirection(new Vector3(0f, -rayLengthDown, 0f));

        Debug.DrawRay(transform.position, down, Color.red);
        if (Physics.Raycast(transform.position, down, rayLengthDown))
        {
            myState = CharacterState.GROUNDED;
            return;
        }

        if ((Physics.Raycast(bottomRight, right, rayLength) ||
           Physics.Raycast(topRight, right, rayLength)))
        {
            myState = CharacterState.WALLGRABRIGHT;
        }

        if ((Physics.Raycast(bottomLeft, left, rayLength) ||
            Physics.Raycast(topLeft, left, rayLength)))
        {
            myState = CharacterState.WALLGRABLEFT;
        }
    }

    //if players state is jumping then these are the movemnts that will run
    public void JumpingMovement()
    {
        if (body.velocity.x > 0)
        {
            if (Input.GetKey("a") && timercheck != true)
            {
                var amount = -speed * Time.deltaTime * 60f;
                var temp = body;
                temp.velocity = new Vector3(amount, body.velocity.y, 0f);
                if (temp.velocity.x < 5.0f)
                {
                    body.velocity = new Vector3(amount, body.velocity.y, 0f);
                }
            }
        }
        else if(body.velocity.x < 0)
        {
           if (Input.GetKey("d") && timercheck != true)
           {
               var amount = speed * Time.deltaTime * 60f;
               var temp = body;
               temp.velocity = new Vector3(amount, body.velocity.y, 0f);
               if (temp.velocity.x < 5.0f)
               {
                   body.velocity = new Vector3(amount, body.velocity.y, 0f);
               }
           }
        }
        else
        {
            if (Input.GetKey("d"))
            {
                var amount = speed * Time.deltaTime * 60f;
                var temp = body;
                temp.velocity = new Vector3(amount, body.velocity.y, 0f);
                if (temp.velocity.x < 5.0f)
                {
                    body.velocity = new Vector3(amount, body.velocity.y, 0f);
                }
            }

            if (Input.GetKey("a"))
            {
                var amount = -speed * Time.deltaTime * 60f;
                var temp = body;
                temp.velocity = new Vector3(amount, body.velocity.y, 0f);
                if (temp.velocity.x < 5.0f)
                {
                    body.velocity = new Vector3(amount, body.velocity.y, 0f);
                }
            }
        }
    }

    //if the player is touch a wall then these are the movements that will happen, depending on the side touching the jump will change
    public void WallGrabMovement(bool direction) //true = right, false = left
    {
        jump = 0;
        if (direction)
        {
            //when the player is gripping using the right side
            if (Input.GetKeyDown("space") && jump < maxJumps)
            {
                var amount = jumpHeight * Time.deltaTime * 100.0f;
                body.AddForce(-6.0f, amount * 1.2f, 0.0f, ForceMode.Impulse);
                myState = CharacterState.JUMPING;
                jump++;
                timercheck = true;
                StartCoroutine(WallJumpTimer());
            }
            if (Input.GetKey("d"))
            {
                var amount = -1 * Time.deltaTime * 75f;
                var temp = body;
                temp.velocity = new Vector3(0f, amount, 0f);
                if (temp.velocity.x < 5.0f)
                {
                    body.velocity = new Vector3(0.0f, amount, 0f);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown("space") && jump < maxJumps)
            {
                var amount = jumpHeight * Time.deltaTime * 100.0f;
                body.AddForce(6.0f, amount * 1.2f, 0.0f, ForceMode.Impulse);
                myState = CharacterState.JUMPING;
                jump++;
                timercheck = true;
                StartCoroutine(WallJumpTimer());
            }
            if (Input.GetKey("a"))
            {
                var amount = -1 * Time.deltaTime * 75f;
                var temp = body;
                temp.velocity = new Vector3(0f, amount, 0f);
                if (temp.velocity.x < -5.0f)
                {
                    body.velocity = new Vector3(0.0f, amount, 0f);
                }
            }
        }
    }

    //default movement that runs
    public void GroundedMovement()
    {
        if (Input.GetKey("d"))
        {
            var amount = speed * Time.deltaTime * 100.0f;
            var temp = body;
            temp.velocity = new Vector3(amount, body.velocity.y, 0f);
            if (temp.velocity.x < 5.0f)
            {
                body.velocity = new Vector3(amount, body.velocity.y, 0f);
            }
        }
        if (Input.GetKey("a"))
        {
            var amount = -speed * Time.deltaTime * 100.0f;
            var temp = body;
            temp.velocity = new Vector3(amount, body.velocity.y, 0f);
            if (temp.velocity.x < -5.0f)
            {
                body.velocity = new Vector3(amount, body.velocity.y, 0f);
            }
        }
        if (Input.GetKey("space") && jump < maxJumps)
        {
            var amount = jumpHeight * Time.deltaTime * 100.0f;
            body.AddForce(0.0f, amount, 0.0f, ForceMode.Impulse);
            myState = CharacterState.JUMPING;
            jump++;

        }
    }

    //resets the jump count to 0 aslong as the player is colliding with something
    public void OnCollisionStay(Collision collision)
    {
        jump = 0;
    }
}

