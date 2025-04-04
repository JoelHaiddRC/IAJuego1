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

    private void Start()
    {
        if (!transform.name.Contains("Initial")) //Todos los cuartos menos el inicial estarán apagados
        {
            TurnOffRoom();
        }
        else
        {
            TurnOnRoom();
        }
    }

    public void PlayerTeleported(bool value)
    {
        isPlayerHere = value;
    }

    void TurnOnRoom()
    {
        VirtualCam.Priority = 20;
        off_screen.SetActive(false);
        StartCoroutine("Wait");
    }

    void TurnOffRoom() {
        off_screen.SetActive(true);
        roomObjects.SetActive(false);
        VirtualCam.Priority = 10;
        canSpawnEnemies = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger
            && isPlayerHere)
        {
            TurnOnRoom();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        roomObjects.SetActive(true);
        canSpawnEnemies = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger
            && !isPlayerHere)
        {
            TurnOffRoom();
        }
    }
}
