using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    private Player player;
    private LifeSystem playerLife;
    private bool gameOver;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        playerLife = player.gameObject.GetComponent<LifeSystem>();
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
}
