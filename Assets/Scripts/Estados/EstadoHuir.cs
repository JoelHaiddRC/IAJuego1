using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoHuir : Estado
{

    
    public Transform objetivo;
    public Rigidbody2D rbObjetivo;
    private Animator animacionPersonaje;

    public Transform personaje;
    public Rigidbody2D rbPersonaje;

    private float velMaxima;
    private float distanciaSegura;

    public EstadoHuir(Transform _objetivo, Rigidbody2D _rbObjetivo, Transform _personaje, Rigidbody2D _rbPersonaje, float velocidadMax, float distancia, Animator animator)
    {
        velMaxima = velocidadMax;
        distanciaSegura = distancia;
        objetivo = _objetivo;
        rbObjetivo = _rbObjetivo;
        personaje = _personaje;
        rbPersonaje = _rbPersonaje;
        animacionPersonaje = animator;
    }


    /**
      * Realiza el movimiento de Flee mejordado.
      * Se modifica la velocidad del objeto para tener un movimiento basado en fisica.
      */
    public void Tick()
    {
        Vector2 posicion = new Vector2(personaje.position.x, personaje.position.y);
        Vector2 posicionObj = new Vector2(objetivo.position.x, objetivo.position.y);
        Vector2 direccion = posicion - posicionObj;

        if (direccion.magnitude < distanciaSegura){

            Vector2 velocidadMax = direccion.normalized * velMaxima;

            Vector2 velocidadActual = rbPersonaje.velocity;

            Vector2 diffVelocidad = velocidadMax - velocidadActual;

            velocidadActual += (diffVelocidad * Time.deltaTime);
            velocidadActual = Vector2.ClampMagnitude(velocidadActual, velMaxima);

            // Comentar la primera linea y descomentar la segunda para usar translate
            rbPersonaje.velocity = velocidadActual;
            animacionPersonaje.SetFloat("MoveX", velocidadActual.x);
            animacionPersonaje.SetFloat("MoveY", velocidadActual.y);
            //this.transform.Translate(velocidadActual.x , velocidadActual.y, 0.0f, Space.World);
        }
        else
        {
            rbPersonaje.velocity = Vector2.zero;
        }

    }

    
    public void OnEnter()
    {
        if (personaje.gameObject.name.Contains("Contact_Enemy"))
            personaje.gameObject.GetComponent<Animator>().SetTrigger("Flee");
    }

    public void OnExit()
    {

    }
    
}
