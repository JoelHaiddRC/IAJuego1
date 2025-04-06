using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public bool isPaused;
    public bool canPause;
    public GameObject pauseMenu;
    private PlayerState previousState;
    private Player player;
    private LevelManager progress;
    void Start()
    {
        isPaused = false;
        canPause = true;
        player = GameObject.FindObjectOfType<Player>();
        progress = GameObject.FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        if (!canPause)
            return;
        if (player.currentState != PlayerState.PAUSED)
            previousState = player.currentState;

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                isPaused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
        if (isPaused)
        {
            player.currentState = PlayerState.PAUSED;
            if (Input.GetKeyDown(KeyCode.R))
                ResetGame();
            if (Input.GetKeyDown(KeyCode.Q))
                Quit();
        }


    }

    public void Resume()
    {
        Debug.Log("Resume");
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        player.currentState = previousState;
    }

    public void ResetGame()
    {
        Debug.Log("Reset");
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        progress.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
