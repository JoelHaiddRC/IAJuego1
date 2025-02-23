using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isVisible;
    public bool canChange;
    public bool canBeKnockbacked;
    public GameObject drop;
    public int dropChance = 0;
    public int exp;
    public EnemyState currentState;
    protected LifeSystem lifeSystem;
    protected RoomTransition room;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        if (hit != null && hit.CompareTag("PlayerAttack"))
        {
            lifeSystem.DamageObject();
            if(canBeKnockbacked)
                lifeSystem.KnockBack(hit);
            if (lifeSystem.life == 0)
            {
                DropItem();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exits");
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    private void DropItem()
    {
        if (drop == null)
            return;
        if (dropChance > 100 || dropChance < 0)
            Debug.LogWarning("Invalid value in dropChance for: " + gameObject.name);
        if (Random.Range(0, 100) < dropChance)
            Instantiate(drop, transform.position, Quaternion.identity);
        drop = null;
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }


}
