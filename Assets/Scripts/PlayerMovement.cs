using Unity.Services.Analytics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    private CharacterController controller;
    public float playerLife = 100;
    public GameObject sphere;
    public Transform Enemy;
    public Transform Player;
    bool isDead = false;
    

    //Walking Animations
    public Animator player;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpheight = 3.5f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        GameObject zone = new GameObject("InvisZone");
        zone.transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Ground check with spehere
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0) { velocity.y = -2f; }

        //Getting the inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Create the moving vector
        Vector3 move = transform.right * x + transform.forward * z;

        //Moving the player
        controller.Move(move * speed * Time.deltaTime);

        //Jump check
        if (Input.GetButtonDown("Jump") && isGrounded) { velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity); }

        //gravity :) (I hate this: o vectors o lalala ijfeoisej)
        velocity.y += gravity * Time.deltaTime;

        //Executing our jumpi jump
        controller.Move(velocity * Time.deltaTime);

        //I am going isane...
        //isMoving

        if (Vector3.Distance(lastPosition, transform.position) > 0.05f && isGrounded)
        {
            isMoving = true;
            player.SetBool("isWalking", true);

            //for later (sprint)
        }
        else
        {
            isMoving = false;
            player.SetBool("isWalking", false);
        }

        lastPosition = transform.position;

        

       
    }

    
}
