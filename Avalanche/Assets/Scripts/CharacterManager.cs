using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public List<GameObject> playerList = new List<GameObject>();
    private List<Material> skinList = new List<Material>();
    private int currentSkin;
    private int previousSkin;
    private string sceneName;

    //menu specific variables
    public GameObject Canvas;
    private GameObject RightArrow;
    private GameObject LeftArrow;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        currentSkin = PlayerPrefs.GetInt("Skin");
        previousSkin = PlayerPrefs.GetInt("Skin");

        //runs menu specific start
        switch (sceneName)
        {
            case "Menu":
                {
                    MainMenuStart();
                    break;
                }
            default:
                Canvas = null;
                break;
        }


        var tempSkinArray = Resources.LoadAll<Material>("Materials/PlayerSkins");
        foreach(var i in tempSkinArray)
        {
            skinList.Add(i);
        }
        if(currentSkin < 0 || currentSkin > skinList.Count)
        {
            currentSkin = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //runs menu update
        switch (sceneName)
        {
            case "Menu":
                {
                    MainMenuUpdate();
                    break;
                }
        }

        //if current skin has changed
        //saves setting and getting the player prefs each update
        if (previousSkin != currentSkin)
        {
            PlayerPrefs.SetInt("Skin", currentSkin);
        }

        foreach(var player in playerList)
        {
            MeshRenderer currentMaterial = player.GetComponent<MeshRenderer>();
            string[] name = currentMaterial.material.name.Split();

            if (name[0] != skinList[currentSkin].name)
            {
                currentMaterial.material = skinList[currentSkin];
            }
        }
    }

    /*
        Anything below this will only run if the 
        scene name is exactly "Menu"     
    */

    private void MainMenuStart()
    { 
        var CharacterMenu = Canvas.transform.Find("CharacterMenu").gameObject;
        RightArrow = CharacterMenu.transform.Find("CharacterRightArrow").gameObject;
        LeftArrow = CharacterMenu.transform.Find("CharacterLeftArrow").gameObject;
    }

    private void MainMenuUpdate()
    {
        CharacterArrowStates();
    }

    //disables and enables the states of the arrows when at start of skinlist and at end
    public void CharacterArrowStates()
    {
        if ((currentSkin + 1) > skinList.Count - 1)
        {
            RightArrow.SetActive(false);
        }
        else
        {
            RightArrow.SetActive(true);

        }

        if (currentSkin - 1 < 0)
        {
            LeftArrow.SetActive(false);
        }
        else
        {
            LeftArrow.SetActive(true);
        }
    }

    //using the arrow buttons change the skin
    public void IncrementSkinNumber()
    {
        previousSkin = currentSkin;
        currentSkin++;
    }
    public void DecrementSkinNumber()
    {
        previousSkin = currentSkin;
        currentSkin--;
    }
}
