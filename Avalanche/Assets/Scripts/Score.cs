using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{ 
    public Text scoreText;
    public Text hiscore;
    private float x = 0;
    private GameObject player;
    
    private void Start()
    {
        PlayerPrefs.SetFloat("Hiscore", 0f);
        player = GetComponent<CharacterManager>().playerList[0];
        //get player grabs the player first and only player in playerList on the character manager
        player = GetComponent<CharacterManager>().playerList[0];
        hiscore.text = "Hiscore: " + (int)PlayerPrefs.GetFloat("Hiscore");
    }
    
    public void Update()
    {
    

        float number = player.transform.position.y;

        if (number >= PlayerPrefs.GetFloat("Hiscore"))
        {

            PlayerPrefs.SetFloat("Hiscore", number);
            hiscore.text = "Hiscore: " + (int)number;
        }
    
        scoreText.text = "Current Score: " + (int)player.transform.position.y;
    }
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    
    }

} 
