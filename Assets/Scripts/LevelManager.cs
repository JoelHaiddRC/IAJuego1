using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private Player player;
    private bool gameOver;
    public GameObject winScreen;
    public Teleporter door;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if(!player.isAlive && !gameOver)
        {
            gameOver = true;
            StartCoroutine("GameOver");
            return;
        }


    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameWin()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnlockDoor()
    {
        door.isClosed = false;
        door.openDoor();
    }

    public void UnlockAllDoors()
    {
        Teleporter[] doors = GameObject.FindObjectsOfType<Teleporter>();
        for(int i = 0; i < doors.Length; i++)
        {
            doors[i].isClosed = false;
            doors[i].openDoor();
        }
        {

        }
    }
}
