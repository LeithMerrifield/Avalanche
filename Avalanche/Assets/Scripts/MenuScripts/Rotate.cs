using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        Quaternion temp = new Quaternion(0f, rotationSpeed * Time.deltaTime,0f,0f);
        transform.Rotate((new Vector3(0f, -1f, 0)) * rotationSpeed * Time.deltaTime * 50f);
    }
}
