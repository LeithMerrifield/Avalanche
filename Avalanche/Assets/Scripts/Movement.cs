using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Movement : MonoBehaviour
{
    public float jumpHeight;
    public float speed;
    public int maxJumps;
    public eCharacterState myState;
    public float jumpDelay = 0.1f;

    private Rigidbody body;
    private int jump;
    private bool timercheck = false;
    private bool tempcheck = true;
    private float rayLength = .3f;
    private float rayLengthDown = 1f;
    private bool jumpingState;
    private Animator playAnim;

    public enum eCharacterState
    {
        GROUNDED,
        JUMPING,
        WALLGRABRIGHT,
        WALLGRABLEFT
    }

    // Start is called before the first frame update
    void Start()
    {
        playAnim = GetComponent<Animator>();
        myState = eCharacterState.GROUNDED;
        body = GetComponent<Rigidbody>();
    }

    IEnumerator WallJumpTimer()
    {
        yield return new WaitForSeconds(.2f);
        timercheck = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Raycasts();
        switch (myState)
        {
            case eCharacterState.JUMPING:
                {
                    JumpingMovement();
                    break;
                }
            case eCharacterState.WALLGRABRIGHT:
                {
                    WallGrabMovement(true);
                    break;
                }
            case eCharacterState.WALLGRABLEFT:
                {
                    WallGrabMovement(false);
                    break;
                }
            case eCharacterState.GROUNDED:
                {
                    GroundedMovement();
                    break;
                }
        }
    }

    public void Raycasts()
    {
        Vector3 topRight = new Vector3(transform.position.x, transform.position.y + .45f, transform.position.z);
        Vector3 bottomRight = new Vector3(transform.position.x, transform.position.y - .45f, transform.position.z);
        Vector3 topLeft = new Vector3(transform.position.x, transform.position.y + .45f, transform.position.z);
        Vector3 bottomLeft = new Vector3(transform.position.x, transform.position.y - .45f, transform.position.z);

        Vector3 right = transform.TransformDirection(new Vector3(rayLength, 0f, 0f));
        Vector3 left = transform.TransformDirection(new Vector3(-rayLength, 0f, 0f));
        Vector3 down = transform.TransformDirection(new Vector3(0f, -rayLengthDown, 0f));

        if ((Physics.Raycast(transform.position, down, rayLengthDown)) && jumpingState == false)
        {
            myState = eCharacterState.GROUNDED;
            return;
        }

        if ((Physics.Raycast(bottomRight, right, rayLength) ||
           Physics.Raycast(topRight, right, rayLength)))
        {
            myState = eCharacterState.WALLGRABRIGHT;
        }

        if ((Physics.Raycast(bottomLeft, left, rayLength) ||
            Physics.Raycast(topLeft, left, rayLength)))
        {
            myState = eCharacterState.WALLGRABLEFT;
        }
    }

    public void JumpingMovement()
    {
        if (body.velocity.x > 0)
        {
            if (Input.GetKey("a"))
            {
                var amount = -speed * Time.deltaTime * 75f;
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
                var amount = speed * Time.deltaTime * 75f;
                var temp = body;
                temp.velocity = new Vector3(amount, body.velocity.y, 0f);
                if (temp.velocity.x < 5.0f)
                {
                    body.velocity = new Vector3(amount, body.velocity.y, 0f);
                }
            }
        }
    }

    public void WallGrabMovement(bool direction) //true = right, false = left
    {
        if(direction)
        {

        }
    }

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
            if (temp.velocity.x < 5.0f)
            {
                body.velocity = new Vector3(amount, body.velocity.y, 0f);
            }
        }

        if (Input.GetKey("space") && jump < maxJumps)
        {
            StartCoroutine(Jump());
            myState = eCharacterState.JUMPING;
            jump++;
            jumpingState = true;
        }
    }

    private IEnumerator Jump()
    {
        playAnim.Play("JumpTest", 0, 0.25f);
        yield return new WaitForSeconds(jumpDelay);
        var amount = jumpHeight * Time.deltaTime * 100.0f;
        body.AddForce(0.0f, amount, 0.0f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        jumpingState = false;
        jump = 0;
    }
}

