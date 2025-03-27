using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase padre de Enemigos, todos los demás deben heredar de esta clase

//Estados de los enemigos
public enum EnemyState
{
    IDLE,
    WALK,
    ATTACK,
    DAMAGED,
    PAUSED
}

[RequireComponent(typeof(LifeSystem))]
public class Enemy : MonoBehaviour
{
    public float walkSpeed;
    public bool isAlive;
    public bool isVisible; //Indica si el jugador está en el mismo cuarto
    public bool canChange; //Indica si puede cambiar de estado
    public bool canBeKnockbacked; //Indica si es posible empujarlo
    public EnemyState currentState;
    protected LifeSystem lifeSystem;
    protected RoomTransition room; //El cuarto en el que se encuentra

    //Este método se hará un override si se escribe en la clase hijo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        if (hit != null && hit.CompareTag("PlayerAttack"))
        {
            lifeSystem.DamageObject();
            if(canBeKnockbacked)
                lifeSystem.KnockBack(hit);
        }
    }

    //Este método se hará un override si se escribe en la clase hijo
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    //Método para cambiar de estado y evitar cambios interminables al mismo estado
    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }


}
