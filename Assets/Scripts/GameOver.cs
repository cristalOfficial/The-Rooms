using UnityEngine;

public class GameOver : MonoBehaviour
{

    public gameManage gameManage;
    public EnemyAi player;
    public PlayerMovement health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health.playerLife <= 0 && !player.isDead)
        {
            player.isDead = true;
            gameManage.gameOver();
        }
    }
}
