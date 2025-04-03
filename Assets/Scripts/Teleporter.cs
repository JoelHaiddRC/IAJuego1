using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public bool isClosed;
    public RoomTransition entryRoom;
    private RoomTransition exitRoom;
    private Vector2 exitPoint;
    public Teleporter exit;
    private Player player;
    private SpriteRenderer sprite;
    private BoxCollider2D box;
    public Transform spawnPoint;
    LevelManager level;
    public bool isFinal;

    private void Start()
    {
        if (exit == null)
            Debug.LogWarning("Warning, teleporter: " + name + " doesn't have any exit");
        else
        {
            exitPoint = exit.spawnPoint.position;
            exitRoom = exit.entryRoom;
        }

        if (exitRoom == null || entryRoom == null)
            Debug.LogWarning("Warning, teleporter: " + name + "didn't find RoomTransition scripts");

        player = GameObject.FindObjectOfType<Player>();
        sprite = transform.GetComponent<SpriteRenderer>();
        box = transform.GetComponent<BoxCollider2D>();
        level = GameObject.FindObjectOfType<LevelManager>();

        if (!isClosed)
            openDoor();
    }

    public void openDoor()
    {
        if (isClosed)
            return;
        sprite.color = Color.clear;
        box.isTrigger = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isFinal)
                StartCoroutine("Teleporting");
            else
                level.GameWin();
        }
    }

    private IEnumerator Teleporting()
    {
        entryRoom.isPlayerHere = false;
        player.currentState = PlayerState.PAUSED;
        player.transform.position = exitPoint;
        exitRoom.isPlayerHere = true;
        yield return new WaitForSeconds(1f);
        player.currentState = PlayerState.WALK;
    }
}
