using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.TerrainAPI;
using UnityEditorInternal;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int maxJumps;
    public float jumpHeight;
    public float horizontalSpeed;
    private float horizontalSpeedMulti = 20.0f;
    private bool wallGrabActivated = false;
    public float wallGrabSpeed = 5.0f;
    public float maxFallSpeed = 10.0f;

    public CharacterState myState;
    private bool skipRaycast = false;


    private int jump;
    private bool timercheck = false;
    private float characterHeight;
    private float characterWidth;
    private Rigidbody rb;
    private BoxCollider bCollider;

    public bool[] inputArray = new bool[10]; // 0 = Space, 1 = left, 2 = right

    //the states the player can be in
    public enum CharacterState
    {
        GROUNDED,
        FALLING,
        JUMPING,
        WALLGRABRIGHT,
        WALLGRABLEFT
    }

    // Start is called before the first frame update, or on creation of the object
    void Start()
    {
        myState = CharacterState.GROUNDED;
        rb = GetComponent<Rigidbody>();
        bCollider = GetComponent<BoxCollider>();
        characterHeight = bCollider.bounds.extents.y;
        characterWidth = bCollider.bounds.extents.x;
    }

    // Update is called once per frame and is used for physics updates
    void FixedUpdate()
    {
        if(!skipRaycast)
        {
            Raycasts();
        }

        TeleWallRaycast();
        switch (myState)
        {
            case CharacterState.JUMPING:
                {
                    JumpingMovement();
                    break;
                }
            case CharacterState.WALLGRABRIGHT:
                {
                    GroundedMovement();
                    break;
                }
            case CharacterState.WALLGRABLEFT:
                {
                    GroundedMovement();
                    break;
                }
            case CharacterState.GROUNDED:
                {
                    GroundedMovement();
                    break;
                }
            case CharacterState.FALLING:
                {
                    GroundedMovement();
                    break;
                }
        }
    }

    void Update()
    {
        PlayerInput();
    }

    public void PlayerInput()
    {
        for(int i = 0; i < inputArray.Length; i++)
        {
            inputArray[i] = false;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            inputArray[0] = true;
            myState = CharacterState.JUMPING;
            skipRaycast = true;
        }

        var h = Input.GetAxisRaw("Horizontal");
        if (h < 0)
        {
            inputArray[1] = true;
        }
        else if(h > 0)
        {
            inputArray[2] = true;
        }
    }

    public void GroundedMovement()
    {
        var h = Input.GetAxisRaw("Horizontal");
        Vector3 tempVect = new Vector3(h, 0, 0);
        tempVect = tempVect.normalized * horizontalSpeed * horizontalSpeedMulti * Time.fixedDeltaTime;

        if(wallGrabActivated)
        {
            tempVect.y = wallGrabSpeed;
        }
        else
        {
            if(rb.velocity.y <= maxFallSpeed)
            {
                 tempVect.y = maxFallSpeed;
            }
            else
            {
                tempVect.y = rb.velocity.y;
            }
        }

        rb.velocity = tempVect;
    }

    public void JumpingMovement()
    {
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        myState = CharacterState.FALLING;
        skipRaycast = false;
    }

    public void Raycasts()
    {
        float extraLength = 0.001f;

        Vector3 left = bCollider.bounds.center;
        left.x -= characterWidth - extraLength;
        Vector3 right = bCollider.bounds.center;
        right.x += characterWidth - extraLength;

        Vector3 top = bCollider.bounds.center;
        top.y += characterHeight - extraLength;
        Vector3 bottom = bCollider.bounds.center;
        bottom.y -= characterHeight - extraLength;


        RaycastHit downHit;
        RaycastHit leftHit;
        RaycastHit rightHit;
        Ray leftDownRay = new Ray(left, Vector3.down);
        Ray rightDownRay = new Ray(right, Vector3.down);
        Ray topRightRay = new Ray(top, Vector3.right);
        Ray bottomRightRay = new Ray(bottom, Vector3.right);
        Ray topLeftRay = new Ray(top, Vector3.left);
        Ray bottomLeftRay = new Ray(bottom, Vector3.left);

        Debug.DrawRay(top, new Vector3(-0.26f,0,0), Color.green);
        Debug.DrawRay(bottom, new Vector3(-0.26f, 0, 0), Color.green);
        Debug.DrawRay(left, new Vector3(0, -0.51f, 0), Color.green);
        Debug.DrawRay(right, new Vector3(0, -0.51f, 0), Color.green);
        Debug.DrawRay(top, new Vector3(0.26f, 0, 0), Color.green);
        Debug.DrawRay(bottom, new Vector3(0.26f, 0, 0), Color.green);

        if (myState != CharacterState.GROUNDED &&
            Physics.Raycast(leftDownRay, out downHit, characterHeight + extraLength) ||
            Physics.Raycast(rightDownRay, out downHit, characterHeight + extraLength))
        {
            myState = CharacterState.GROUNDED;
            wallGrabActivated = false;
        }
        else if (myState != CharacterState.GROUNDED && inputArray[1] &&
                 (Physics.Raycast(topLeftRay, out leftHit, characterWidth  + extraLength) ||
                 Physics.Raycast(bottomLeftRay, out leftHit, characterWidth + extraLength)))
        {
            myState = CharacterState.WALLGRABLEFT;
            wallGrabActivated = true;
        }
        else if (myState != CharacterState.GROUNDED && inputArray[2] &&
                 (Physics.Raycast(topRightRay, out rightHit, characterWidth + extraLength) ||
                 Physics.Raycast(bottomRightRay, out rightHit, characterWidth + extraLength)))
        {
            myState = CharacterState.WALLGRABRIGHT;
            wallGrabActivated = true;
        }
        else if (myState != CharacterState.FALLING &&
            (!Physics.Raycast(leftDownRay, out downHit, characterHeight + extraLength) &&
             !Physics.Raycast(rightDownRay, out downHit, characterHeight + extraLength)))
        {
            myState = CharacterState.FALLING;
            wallGrabActivated = false;
        }
    }

    private void TeleWallRaycast()
    {
        float extraWidth = 0.01f;
        RaycastHit HitLeft;
        RaycastHit HitRight;

        Ray leftRay = new Ray(transform.position, Vector3.left);
        Ray rightRay = new Ray(transform.position, Vector3.right);


        if (Physics.Raycast(leftRay, out HitLeft, characterWidth + extraWidth))
        {
            if (HitLeft.collider.tag == "Wall")
            {
                if(!(rb.position.x > 0f))
                {
                    rb.position = new Vector3(-(HitLeft.transform.position.x) - .5f, rb.position.y, rb.velocity.z);
                }
            }
        }
        else if (Physics.Raycast(rightRay, out HitRight, characterWidth + extraWidth))
        {
            if (HitRight.collider.tag == "Wall" && timercheck != true)
            {
                if (!(rb.position.x < 0f))
                {
                    rb.position = new Vector3(-(HitRight.transform.position.x) + .5f, rb.position.y, rb.velocity.z);
                }
            }
        }

    }

    
}

