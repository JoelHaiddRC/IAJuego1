using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 direction;
    public float moveSpeed;
    public float projectileLife;
    private bool fromPlayer;
    void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        fromPlayer = gameObject.CompareTag("PlayerAttack");
        rb.velocity += direction * moveSpeed;
        Destroy(gameObject, 2);
    }
    public void setDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Transform colTrans = collision.transform;
        if ((colTrans.CompareTag("Player") && !fromPlayer)
            || (colTrans.CompareTag("Enemies") && fromPlayer))
        {
            colTrans.gameObject.GetComponent<LifeSystem>().DamageObject();
            Destroy(gameObject);
        }

        if (colTrans.CompareTag("Collisions") || colTrans.CompareTag("PlayerAttack"))
        {
            Destroy(gameObject);
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Collisions") 
            || (collision.transform.CompareTag("PlayerAttack") && !fromPlayer)
            || (collision.transform.CompareTag("Enemies") && fromPlayer))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Transform colTrans = collision.transform;
        if ((colTrans.CompareTag("Player") && !fromPlayer)
            || (colTrans.CompareTag("Enemies") && fromPlayer))
        {
            colTrans.gameObject.GetComponent<LifeSystem>().DamageObject();
            Destroy(gameObject);
        }

        if (colTrans.CompareTag("Collisions") || colTrans.CompareTag("PlayerAttack"))
        {
            Destroy(gameObject);
        }
    }
}
