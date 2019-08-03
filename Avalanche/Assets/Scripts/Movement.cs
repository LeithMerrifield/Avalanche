using System.Collections;
using System.Collections.Generic;
using UnityEngine;







public class Movement : MonoBehaviour
{
    public float jumpHeight;
    public float speed;
    private Rigidbody body;
    public int maxJumps;
    private int jump;
    private float rayLength = 1f;
    public FiniteStateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new FiniteStateMachine();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 topRight = new Vector3(transform.position.x, transform.position.y + .45f, transform.position.z);
        Vector3 bottomRight = new Vector3(transform.position.x, transform.position.y - .45f, transform.position.z);
        Vector3 right = transform.TransformDirection(new Vector3(rayLength, 0f, 0f));

        if (Physics.Raycast(bottomRight, right, rayLength))
        {
            Debug.DrawRay(bottomRight, right, Color.green);
        }
        else
        {
            Debug.DrawRay(bottomRight, right, Color.yellow);
        }

        if(Physics.Raycast(topRight, right, rayLength))
        {
            Debug.DrawRay(topRight, right, Color.green);
        }
        else
        {
            Debug.DrawRay(topRight, right, Color.yellow);
        }



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

        if (Input.GetKeyDown("space"))
        {
            if(jump < maxJumps)
            {
                var amount = jumpHeight * Time.deltaTime * 100.0f;
                body.AddForce(0.0f, amount, 0.0f, ForceMode.Impulse);
                jump++;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        jump = 0;
    }
}
