using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueBehavior : SteeringBehavior
{
    
    [SerializeField] private Rigidbody2D rbObjetivo;
    [SerializeField] private float prediccion; 
    [SerializeField] private float distanciaParo; 

    /**
     * Realiza el movimiento de Seek mejordado.
     * Se modifica la velocidad del objeto para tener un movimiento basado en fisica.
     */
    public override void Execute()
    {
        if (objetivo != null) {
            
            Vector2 posicion = new Vector2(transform.position.x, transform.position.y);
            Vector2 posicionObj = new Vector2(objetivo.position.x, objetivo.position.y);
            
            Vector2 distObj = posicion - posicionObj;

            posicionObj += rbObjetivo.velocity * prediccion;
            
            Vector2 direccion = posicionObj -posicion;

            if (distObj.magnitude > distanciaParo){

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


}
