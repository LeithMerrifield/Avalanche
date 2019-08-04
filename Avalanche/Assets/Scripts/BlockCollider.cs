using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
   
    public GameObject fallingBlockLarge;
    private RisingFloor FallingBlocks;

    public void Start()
    {
        FallingBlocks = GetComponent<RisingFloor>();
    
    }
    void OnCollisionEnter(Collision Info)
    {
        if (Info.collider.tag == "Block")
        {
            FallingBlocks.enabled = false;
        }
        
            
        
        
        
    }
}
