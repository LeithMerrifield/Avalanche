using UnityEngine.SceneManagement;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public float restartDelay = 1f;
    bool gameHasEnded = false;
    public void GameOver()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            
        }

        
    }
    public void Restart()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}