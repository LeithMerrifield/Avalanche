using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float jumpHeight;
    public float speed;
    private Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetKeyDown("space"))
        {

            var amount = jumpHeight * Time.deltaTime * 100.0f;
            body.AddForce(0.0f, amount, 0.0f, ForceMode.Impulse);
        }
    }
}
