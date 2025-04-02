using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveBehavior : SteeringBehavior
{

    [SerializeField] private float radioLento;
    [SerializeField] private float radioParo;


    public override void Execute()
    {
        
        Debug.Log("ArriveBehavior");

        if (objetivo != null) {
            Vector2 posicion = new Vector2(transform.position.x, transform.position.y);
            Vector2 posicionObj = new Vector2(objetivo.position.x, objetivo.position.y);
            Vector2 direccion = posicionObj -posicion ;


            float distanciaObj = direccion.magnitude;
            //Debug.DrawRay(transform.position, direccion, Color.blue);

            if( distanciaObj > radioParo) {
                float velocidadActual = velMaxima;

                if (distanciaObj < radioLento) {
                    velocidadActual = velMaxima * (distanciaObj / radioLento);
                }
                direccion = direccion.normalized * velocidadActual;
                GetComponent<Rigidbody2D>().velocity = direccion;
                //this.transform.Translate(direccion.x * Time.deltaTime, direccion.y * Time.deltaTime, 0.0f, Space.World);


            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
    
    
}
