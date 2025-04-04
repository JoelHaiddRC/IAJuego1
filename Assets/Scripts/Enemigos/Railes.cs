using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railes : MonoBehaviour
{
    public Transform[] waypoints;
    private int waypointActual;
    private Vector3 waypointPosition;
    public float velocidad;
    float velocidadActual;
    public float tiempoAtaque;
    public float tiempoRecarga;
    bool canAttack;
    Animator animator;
    void Start()
    {
        if (waypoints.Length < 1)
        {
            Debug.LogWarning("Waypoints in: " + transform.name + " not defined");
            return;
        }
        waypointPosition = waypoints[0].position;
        waypointActual = 0;
        velocidadActual = velocidad;
        animator = GetComponent<Animator>();
        canAttack = true;
    }

    private void FixedUpdate()
    {
       transform.position = Vector2.MoveTowards(transform.position, waypointPosition, velocidadActual * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        //Si no hay waypoints no se mueve
        if (waypoints.Length < 1 || waypointActual < 0)
            return;
        float distancia = Vector3.Distance(transform.position, waypointPosition);

        //Parte de la clase WaypointFollower del laboratorio
        if (distancia < .1f)
        {
            waypointActual = (waypointActual + 1) % waypoints.Length;
            waypointPosition = waypoints[waypointActual].transform.position;
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && canAttack)
        {
            canAttack = false;
            velocidadActual = 0;
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        animator.SetTrigger("Charge");
        yield return new WaitForSeconds(tiempoAtaque);
        animator.SetTrigger("Attack");
        gameObject.tag = "Enemies";
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Patrol");
        gameObject.tag = "Untagged";
        velocidadActual = velocidad;
        yield return new WaitForSeconds(tiempoRecarga);
        canAttack = true;
    }
}
