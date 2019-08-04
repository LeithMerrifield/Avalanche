using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingFloor : MonoBehaviour
{
    private Rigidbody body;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 RisingFloor = new Vector3(transform.position.x, transform.position.y + (speed * Time.deltaTime), 0f);
        body.transform.position = RisingFloor;
    }
}