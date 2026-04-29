using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    //variables
    public float mouseSesetivity = 100f;
    float xRotation = 0f;
    float yRotation = 0f;
    public float clampUp = -90f;
    public float clampDown = 90f;


    void Start()
    {
        //Locking the cursor to the center of the screen and making it Invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSesetivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSesetivity * Time.deltaTime;

        //Moving mouse:

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, clampUp, clampDown);

        yRotation += mouseX;

        //Aplying to the transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
