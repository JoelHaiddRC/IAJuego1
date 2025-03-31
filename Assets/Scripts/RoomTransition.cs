using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//Clase para hacer "cambio de cuartos", mover la cámara y activar/desactivar sus objetos
public class RoomTransition : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCam;
    public GameObject roomObjects;
    public GameObject off_screen;
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
            off_screen.SetActive(false);
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
            off_screen.SetActive(true);
            roomObjects.SetActive(false);
            VirtualCam.Priority = 10;
            canSpawnEnemies = false;
        }
    }
}
