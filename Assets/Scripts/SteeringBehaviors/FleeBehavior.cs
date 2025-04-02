using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehavior : SteeringBehavior
{
    
    [SerializeField] private float distanciaSegura; 

    /**
     * Realiza el movimiento de Flee mejordado.
     * Se modifica la velocidad del objeto para tener un movimiento basado en fisica.
     */
    public override void Execute()
    {
        Vector2 posicion = new Vector2(transform.position.x, transform.position.y);
        Vector2 posicionObj = new Vector2(objetivo.position.x, objetivo.position.y);
        Vector2 direccion = posicion - posicionObj;

        if (direccion.magnitude < distanciaSegura){

            Vector2 velocidadMax = direccion.normalized * velMaxima;

            Vector2 velocidadActual = GetComponent<Rigidbody2D>().velocity;

            Vector2 diffVelocidad = velocidadMax - velocidadActual;

            velocidadActual += (diffVelocidad * Time.deltaTime);
            velocidadActual = Vector2.ClampMagnitude(velocidadActual, velMaxima);

            // Comentar la primera linea y descomentar la segunda para usar translate
            GetComponent<Rigidbody2D>().velocity = velocidadActual;
            //this.transform.Translate(velocidadActual.x , velocidadActual.y, 0.0f, Space.World);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

    }
    
}
