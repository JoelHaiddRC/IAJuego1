using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase del enemigo default
public class ContactEnemy : Enemy
{
    private Animator animator;

    public float velMaxima; 
    public float distanciaCambio; 
    public float velRotacion; 
    public GameObject[] waypoints;
    Vector2 waypointPosition, velocidad;
    int waypointActual;
    void Start()
    {
        lifeSystem = GetComponent<LifeSystem>();
        animator = GetComponent<Animator>();
        lifeSystem.resetState();
        currentState = EnemyState.WALK;
        canChange = true;
        isVisible = true;
        isAlive = true;
        if(waypoints.Length < 1)
        {
            Debug.LogWarning("Waypoints in: " + transform.name + " not defined");
            return;
        }
        waypointPosition = waypoints[0].transform.position;
        waypointActual = 0;
    }

    //M�todo tomado de WaypointFollower del laboratorio y adaptado al 2D 
    void Seek(Vector3 objetivo)
    {
        Vector2 velocidadDeseada = (objetivo - transform.position).normalized * velMaxima;
        Vector2 fuerza = velocidadDeseada - velocidad;
        velocidad += fuerza * Time.deltaTime;
        velocidad = Vector2.ClampMagnitude(velocidad, velMaxima);
        transform.position = new Vector2(transform.position.x + velocidad.x * Time.deltaTime, 
            transform.position.y + velocidad.y * Time.deltaTime);
        Debug.DrawRay(transform.position, velocidad, Color.red);
        Debug.DrawLine(transform.position, objetivo, Color.green);
    }


    private void FixedUpdate()
    {
        //Si el jugador no est� en el cuarto o hay pausa no se mueve
        if (!isVisible || currentState == EnemyState.PAUSED)
            return;
        if (isAlive && canChange) //Temporal, la condici�n para patrullar puede cambiar
            Seek(waypointPosition);
    }

    void Update()
    {
        //Si el jugador no est� en el cuarto no actualiza nada
        if (!isVisible)
            return;
        isAlive = !lifeSystem.isDead;

        //Si no hay waypoints no se mueve
        if (waypoints.Length < 1 || waypointActual < 0)
            return;
        float distancia = Vector3.Distance(transform.position, waypointPosition);

        //Actualiza la direcci�n en la que se mueve para el animator
        animator.SetFloat("MoveX", velocidad.normalized.x);
        animator.SetFloat("MoveY", velocidad.normalized.y);

        //Parte de la clase WaypointFollower del laboratorio
        if (distancia < distanciaCambio)
        {
            waypointActual = (waypointActual + 1) % waypoints.Length;
            waypointPosition = waypoints[waypointActual].transform.position;
            return;
        }
    }
}
