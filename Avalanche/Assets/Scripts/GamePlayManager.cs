using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayManager : MonoBehaviour
{
    public bool GameEnd = false;
    public List<GameObject> Screens = new List<GameObject>();
    private Dictionary<string, GameObject> screenDictionary = new Dictionary<string, GameObject>();
    private List<GameObject> playerList;

    private void Start()
    {
        playerList = GetComponent<CharacterManager>().playerList;
        foreach(var i in Screens)
        {
            screenDictionary.Add(i.name, i);

        }
    }

    private void Update()
    {
        if (GameEnd)
        {
            var playerMovement = playerList[0].GetComponent<Movement>().enabled = false;
            Time.timeScale = 0;
            screenDictionary["Death Screen"].SetActive(true);
            screenDictionary["Playing Screen"].SetActive(false);
        }
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        GameEnd = false;
    }
}