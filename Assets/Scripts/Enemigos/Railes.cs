using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railes : MonoBehaviour
{
    float velocidadActual;
    public float tiempoAtaque;
    public float tiempoRecarga;
    public float tiempoRayo;
    bool canAttack;
    bool canMove;
    Animator animator;
    public Vector3 posicionInicial;
    GameObject ray;

    [Header("PathFollowing")]
    public float velocidadPath;
    public Transform[] waypoints;
    private int waypointActual;
    private Vector3 waypointPosition;

    [Header("Pursue")]
    public float velocidadPursue;
    public float prediccion;
    Player jugador;
    Rigidbody2D rbObjetivo;
    Rigidbody2D rb;

    public bool hardMode;
    void Start()
    {
        if (waypoints.Length < 2)
        {
            Debug.LogWarning("Waypoints in: " + transform.name + " not defined");
            return;
        }
        hardMode = false;
        waypointPosition = waypoints[1].position;
        waypointActual = 1;
        velocidadActual = velocidadPath;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindObjectOfType<Player>();
        ray = transform.GetChild(0).gameObject;
        ray.SetActive(false);
        rbObjetivo = jugador.rb;
        canAttack = true;
        canMove = true;
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;
        if (!hardMode)
        {
            rb.velocity = Vector2.zero;
            transform.position = Vector2.MoveTowards(transform.position, waypointPosition, velocidadActual * Time.deltaTime);
        }
        else
        {
            Pursue();
        }
    }

    void Pursue()
    {
        Vector2 posicion = new Vector2(transform.position.x, transform.position.y);
        Vector2 posicionObj = new Vector2(jugador.transform.position.x, jugador.transform.position.y);

        posicionObj += rbObjetivo.velocity * prediccion;

        Vector2 direccion = posicionObj - posicion;
        Vector2 velocidadMax = direccion.normalized * velocidadPursue;
        Vector2 velocidadActual = rb.velocity;
        Vector2 diffVelocidad = velocidadMax - velocidadActual;

        velocidadActual += (diffVelocidad * Time.deltaTime);
        velocidadActual = Vector2.ClampMagnitude(velocidadActual, velocidadPursue);
        rb.velocity = velocidadActual;
    }
        

        void Update()
    {
        hardMode = jugador.shootAvailable;
        animator.SetBool("Free", hardMode);
        if (!hardMode)
        {
            if (waypoints.Length < 1 || waypointActual < 0)
                return;
            float distancia = Vector3.Distance(transform.position, waypointPosition);

            if (distancia < .1f)
            {
                waypointActual = (waypointActual + 1) % waypoints.Length;
                waypointPosition = waypoints[waypointActual].transform.position;
                return;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && canAttack)
        {
            canAttack = false;
            canMove = false;
            rb.velocity = Vector2.zero;
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        animator.SetTrigger("Charge");
        yield return new WaitForSeconds(tiempoAtaque);
        animator.SetTrigger("Attack");
        ray.SetActive(true);
        yield return new WaitForSeconds(tiempoRayo);
        ray.SetActive(false);
        yield return new WaitForSeconds(tiempoRecarga);
        animator.SetTrigger("Patrol");
        canAttack = true;
        canMove = true;
    }

    private void OnEnable()
    {
        ray = transform.GetChild(0).gameObject;
        ray.SetActive(false);
        waypointPosition = waypoints[1].position;
        waypointActual = 1;
        if (!hardMode)
            velocidadActual = velocidadPath;
        else
            velocidadActual = velocidadPursue;
        canAttack = true;
        canMove = true;
        jugador = GameObject.FindObjectOfType<Player>();
        rbObjetivo = jugador.rb;
    }

    private void OnDisable()
    {
        transform.position = waypoints[0].position;
    }
}
