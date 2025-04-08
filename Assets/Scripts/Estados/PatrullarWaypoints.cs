using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrullarWaypoints : Estado
{
   
    GameObject[] waypoints;
    Vector2 waypointPosition;
    int waypointActual = 0;

    float velMaxima;
    float distanciaCambio;
    
    
    Transform personaje;

    Rigidbody2D rbPersonaje;

    Animator animacionPersonaje;

    public PatrullarWaypoints(GameObject[] _waypoints, float _velMaxima, float _distanciaCambio, Transform _personaje, Rigidbody2D _rbPersonaje, Animator animator)
    {
        waypoints = _waypoints;
        velMaxima = _velMaxima;
        distanciaCambio = _distanciaCambio;
        personaje = _personaje;
        rbPersonaje = _rbPersonaje;
        animacionPersonaje = animator;
        
    }


    public void Tick()
    {
        if (waypoints.Length == 0)
            return;
        
        Vector2 posicion = new Vector2(personaje.position.x, personaje.position.y);
        float distancia = Vector2.Distance(posicion, waypointPosition);
        
        if(distancia < distanciaCambio)
        {
            waypointActual = (waypointActual + 1) % waypoints.Length;
            Vector3 pos = waypoints[waypointActual].transform.position;
            waypointPosition = new Vector2(pos.x, pos.y);
            return;
        }
        Seek(waypointPosition);
    }

    private void Seek(Vector2 posicionObj)
    {
        Vector2 posicion = new Vector2(personaje.position.x, personaje.position.y);
        //Vector2 posicionObj = new Vector2(objetivo.position.x, objetivo.position.y);
        Vector2 direccion = posicionObj - posicion;

        if (direccion.magnitude > distanciaCambio){

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
        //Debug.DrawRay(transform.position, velocidad, Color.red);
        //Debug.DrawLine(transform.position, objetivo, Color.green);
    }

    public void OnEnter() 
    {
        if (waypoints.Length < 1)
        {
            Debug.Log("No Waypoints");
            return;
        }
        Vector3 pos = waypoints[0].transform.position;
        waypointPosition = new Vector2(pos.x, pos.y);
        waypointActual = 0;

        if (personaje.gameObject.name.Contains("Contact_Enemy"))
            personaje.gameObject.GetComponent<Animator>().SetTrigger("Patrol");
    }


    public void OnExit() 
    {

    }


}
