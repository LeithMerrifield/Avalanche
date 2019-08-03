
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Transform player;
    public Text scoreText;
    public Text hiscore;
    public int x = 0;
    public int y = 0;

    private void Start()
    {
        hiscore.text = PlayerPrefs.GetInt("Hiscore", 0).ToString();
    }
    
    public void Update()
    {

        scoreText.text =  player.position.y.ToString("0");
        int.TryParse(scoreText.text, out x);

        int number = x;
        if (number > PlayerPrefs.GetInt("Hiscore", 0))
        {
            PlayerPrefs.SetInt("Hiscore", number);
            hiscore.text = number.ToString();
        }

    }
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    
    }
}
