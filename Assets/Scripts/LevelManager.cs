using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private Player player;
    private bool gameOver;
    public GameObject winScreen;
    public Teleporter door;
    LifeSystem playerLife;
    public int enemiesLeft;
    public int patrol;
    public int orb;
    public int shooter;
    bool robotDefeated;
    bool exitOpen;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI objectivesText;
    public TextMeshProUGUI patrolEnemiesText;
    public TextMeshProUGUI orbsEnemiesText;
    public TextMeshProUGUI shooterEnemiesText;
    public GameObject enemiesLeftObject;
    public GameObject Attacks;
    public GameObject sword;
    public GameObject projectile;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        playerLife = player.transform.GetComponent<LifeSystem>();
        gameOver = false;
        exitOpen = false;
        robotDefeated = false;
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void killedEnemy(int enemyType)
    {
        switch (enemyType)
        {
            case 0:
                patrol--;
                break;
            case 1:
                shooter--;
                break;
            case 2:
                orb--;
                break;
            case 3:
                robotDefeated = true;
                break;
        }
    }

    private void Update()
    {
        lifeText.text = playerLife.life.ToString();
        enemiesLeft = patrol + orb + shooter;

        if (player.attackAvailable)
        {
            Attacks.SetActive(true);
            sword.SetActive(player.swordChar);
            projectile.SetActive(!player.swordChar);
        }
        else
        {
            Attacks.SetActive(false);
        }

        if (player.shootAvailable)
        {
            enemiesLeftObject.SetActive(true);
            patrolEnemiesText.text = patrol.ToString();
            orbsEnemiesText.text = orb.ToString();
            shooterEnemiesText.text = shooter.ToString();
        }

        if (!player.isAlive && !gameOver)
        {
            gameOver = true;
            StartCoroutine("GameOver");
            return;
        }

        if(robotDefeated && !exitOpen)
        {
            exitOpen = true;
            UnlockAllDoors();
        }

        CheckObjetives();
    }

    void CheckObjetives()
    {
        if (!player.attackAvailable)
            objectivesText.text = "Objetivo: Encuentra la espada de energía";
        else
        {
            if (!player.shootAvailable)
                objectivesText.text = "Objetivo: Encuentra el proyectil de energía";
            else
            {
                if (enemiesLeft > 0)
                    objectivesText.text = "Objetivo: Derrota a todos los enemigos para quitarle el escudo al robot";
                else
                {
                    if (!robotDefeated)
                        objectivesText.text = "Objetivo: Derrota al robot";
                    else
                        objectivesText.text = "Objetivo: Escapa por la puerta de salida";
                }
            }
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
        Debug.Log("Unlocked door");
        door.isClosed = false;
        door.openDoor();
    }

    public void UnlockAllDoors()
    {
        Debug.Log("Unlocked all doors");
        Teleporter[] doors = GameObject.FindObjectsOfType<Teleporter>();
        for(int i = 0; i < doors.Length; i++)
        {
            doors[i].isClosed = false;
            doors[i].openDoor();
        }
    }
}
