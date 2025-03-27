using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase de proyectiles disparados por otro objeto
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 direction;
    public float moveSpeed; //Velocidad de movimiento
    public float projectileLife; //Tiempo antes de que se elimine el objeto si no colisiona
    private bool fromPlayer; //Indica si lo disparó el jugador o el enemigo
    void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        fromPlayer = gameObject.CompareTag("PlayerAttack");
        rb.velocity += direction * moveSpeed;
        Destroy(gameObject, projectileLife);
    }
    public void setDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    //Se checa con colisiones de tipo trigger y normal para cubrir jugador, enemigos y otros proyectiles
    private void OnTriggerStay2D(Collider2D collision)
    {
        Transform colTrans = collision.transform;
        if ((colTrans.CompareTag("Player") && !fromPlayer)
            || (colTrans.CompareTag("Enemies") && fromPlayer))
        {
            //Si es del jugador y colisionó con enemigo (o viceversa) baja el daño y se destruye
            colTrans.gameObject.GetComponent<LifeSystem>().DamageObject();
            Destroy(gameObject);
        }

        if (colTrans.CompareTag("Collisions") || colTrans.CompareTag("PlayerAttack"))
        {
            //Si colisionó con algun objeto con la tag "Collisions" u otro proyectil se destruye
            Destroy(gameObject);
        }
            
    }

    //Tiene el mismo comportamiento que OnTriggerStay2D
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
