using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManage : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject titleScreen;
    public PlayerMovement player;

    // Static variables stay the same even when the scene reloads
    private static bool isFirstLoad = true;

    void Start()
    {
        if (isFirstLoad)
        {
            // Only show menu and pause if it's the very first time opening the game
            PreparePlayerForMenu();
        }
        else
        {
            // If restarting, make sure time is running and menu is hidden
            Time.timeScale = 1f;
            titleScreen.SetActive(false);
            gameOverUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void play()
    {
        player.playerLife = 100;
        isFirstLoad = false; // Mark that we have started the game
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        titleScreen.SetActive(false);
    }

    public void restart()
    {
        isFirstLoad = false; // Ensure we stay in "play mode" after restart
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu()
    {
        isFirstLoad = true; // Set back to true so the menu shows up
        PreparePlayerForMenu();
        titleScreen.SetActive(true);
        gameOverUI.SetActive(false);
    }

    private void PreparePlayerForMenu()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = new Vector3(-45.52f, 1.3f, -46.63f);
        player.transform.eulerAngles = new Vector3(0f, 184.658f, 0f);

        if (cc != null) cc.enabled = true;
    }

    public void gameOver()
    {
        Debug.Log("Game Over Triggered!"); // Check your Console for this message

        // Ensure time stops so the player can't keep moving
        Time.timeScale = 0f;

        // Unlock the mouse so you can click the buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Show the UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogError("GameOverUI is missing from the Inspector!");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}