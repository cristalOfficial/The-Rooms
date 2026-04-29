using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    private Vector3 startingPosition;
    public int BobLife = 200;
    public float pathFindDistance = 80f;
    public Transform objective;
    private NavMeshAgent agent;
    public Transform body;
    bool isInAtackRange = false;
    public bool isDead = false;
    

    //Reference to player script
    public PlayerMovement player;

    bool arrived;

    private void Start()
    {
        startingPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        

        if (player.playerLife <= 0)
        {
            gameManage manager = FindObjectOfType<gameManage>();
            isDead = true;
            Die();
        }
        if (BobLife <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!arrived)
        {
            Chasing();
        }
        else
        {
          
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                arrived = false;
                EnemyRoam();
            }
        }
    }

    private Vector3 GetRoamingPosition()
    {
       
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        float distance = Random.Range(10f, pathFindDistance);
        return startingPosition + randomDirection * distance;
    }

    private void EnemyRoam()
    {
        if (objective != null)
        {
            agent.isStopped = false;
            agent.SetDestination(GetRoamingPosition());
        }
    }

    private void Chasing()
    {
        if (objective != null)
        {
            agent.SetDestination(objective.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.transform == objective)
        {
            Debug.Log("objetive: " + other.name);
            arrived = true;
           
        }

        if(other.transform == body)
        {
            if(player != null)
            {
                Debug.Log("objetive : " + other.name);
                player.playerLife -= 15;
                print(player.playerLife);
            }
        }

        
    }

    void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }
}