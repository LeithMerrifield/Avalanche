using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FallingBlocks : MonoBehaviour
{
    public GameObject fallingBlockLarge;
    public GameObject fllingBlockSmall;
    public float spawnTime = 2f;
    public float fallSpeed = 40.0f;
    private float timer = 0;
    private int randomNumber;
    public float distanceBlockFall;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnTime)
        {
            SpawnRandom();
            timer = 0;
        }
        void SpawnRandom()
        {


            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(1000, Screen.height), distanceBlockFall));

            Instantiate(fallingBlockLarge, screenPosition, Quaternion.identity);
        }
    }
}