using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomTransition : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCam;
    public GameObject roomObjects;
    public bool canSpawnEnemies;
    public bool isPlayerHere;

    public void PlayerTeleported(bool value)
    {
        isPlayerHere = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger
            && isPlayerHere)
        {
            VirtualCam.Priority = 20;
            StartCoroutine("Wait");
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        roomObjects.SetActive(true);
        canSpawnEnemies = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger
            && !isPlayerHere)
        {
            roomObjects.SetActive(false);
            VirtualCam.Priority = 10;
            canSpawnEnemies = false;
        }
    }
}
