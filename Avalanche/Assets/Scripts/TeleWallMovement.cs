using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleWallMovement : MonoBehaviour
{
    public GameObject player;
    private Rigidbody body;
    private MeshRenderer mahMesh;
    public bool visual = false; 
    private void Start()
    {
        body = player.GetComponent<Rigidbody>();
        mahMesh = GetComponent<MeshRenderer>();
        mahMesh.enabled = visual;
    }

    public void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, body.transform.position.y, transform.position.z);
    }
}
