using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoAtaque : Estado
{

    
    public Transform objetivo;
    public Rigidbody2D rbObjetivo;

    public Transform personaje;
    public Rigidbody2D rbPersonaje;

    private float velMaxima;
    private float distanciaSegura;

    private Animator animacionPersonaje;

    public EstadoAtaque(Transform _objetivo, Rigidbody2D _rbObjetivo, Transform _personaje, Rigidbody2D _rbPersonaje, float velocidadMax, float distancia, Animator animacion)
    {
        velMaxima = velocidadMax;
        distanciaSegura = distancia;
        objetivo = _objetivo;
        rbObjetivo = _rbObjetivo;
        personaje = _personaje;
        rbPersonaje = _rbPersonaje;
        animacionPersonaje = animacion;
    }


    /**
      * Realiza el movimiento de Seek mejordado.
      * Se modifica la velocidad del objeto para tener un movimiento basado en fisica.
      */
    public void Tick()
    {
        Vector2 posicion = new Vector2(personaje.position.x, personaje.position.y);
        Vector2 posicionObj = new Vector2(objetivo.position.x, objetivo.position.y);
        Vector2 direccion = posicionObj - posicion;

        if (direccion.magnitude > distanciaSegura){

            Vector2 velocidadMax = direccion.normalized * velMaxima;

            Vector2 velocidadActual = rbPersonaje.velocity;

            Vector2 diffVelocidad = velocidadMax - velocidadActual;

            velocidadActual += (diffVelocidad * Time.deltaTime);
            velocidadActual = Vector2.ClampMagnitude(velocidadActual, velMaxima);

            // Comentar la primera linea y descomentar la segunda para usar translate
            rbPersonaje.velocity = velocidadActual;
            animacionPersonaje.SetFloat("MoveX", velocidadActual.x);
            animacionPersonaje.SetFloat("MoveY", velocidadActual.y);
            //animator.SetFloat("LastX", lastDirection.x);
            //animator.SetFloat("LastY", lastDirection.y);
            //this.transform.Translate(velocidadActual.x , velocidadActual.y, 0.0f, Space.World);
        }
        else
        {
            rbPersonaje.velocity = Vector2.zero;
        }

    }

    
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {

    }
    
}
