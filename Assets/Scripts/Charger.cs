using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Charger : Enemy
{
    private Transform target;
    public float chaseDistance;
    public float attackDistance;
    private bool chasing;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lifeSystem = GetComponent<LifeSystem>();
        lifeSystem.resetState();
        currentState = EnemyState.WALK;
        isAlive = true;
        isVisible = true;
        if (walkSpeed < 0)
            walkSpeed = 0;
        target = GameObject.FindWithTag("Player").transform;
        canChange = true;
    }

    void Charge()
    {
        if (currentState != EnemyState.WALK)
            return;

        if (chasing)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, target.position,
                            (walkSpeed / 50) * Time.deltaTime));
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }


    private void FixedUpdate()
    {
        if (!isVisible || currentState == EnemyState.PAUSED)
            return;
        if (isAlive && canChange)
            Charge();
    }

    void Update()
    {
        if (!isVisible)
            return;
        isAlive = !lifeSystem.isDead;


        float playerDistance = Vector2.Distance(target.position, transform.position);
        chasing = playerDistance <= chaseDistance;
    }
}
