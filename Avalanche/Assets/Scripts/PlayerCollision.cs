
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Movement Movement;
    public GameObject RestartButton;
    public void Start()
    {
        Movement = GetComponent<Movement>();
    }
    void OnCollisionEnter(Collision Info)
    {
        if (Info.collider.tag == "Obstacle")
        {
            Movement.enabled = false;
            FindObjectOfType<Manager>().GameOver();
            RestartButton.SetActive(true);
            
        }
        else
        {
            RestartButton.SetActive(false);
        }
    }
}
