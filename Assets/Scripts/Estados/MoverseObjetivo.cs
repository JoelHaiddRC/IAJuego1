using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoverseObjetivo : Estado
{
    
    public Transform objetivo;

    public NavMeshAgent agent;

    private Shooter disparos;

    private Animator animacionPersonaje;


    public MoverseObjetivo(Transform _objetivo, NavMeshAgent _agent, Animator animacion)
    {
        objetivo = _objetivo;
        agent = _agent;
        animacionPersonaje = animacion;
    }


    /**
      * Realiza el movimiento de Seek mejordado.
      * Se modifica la velocidad del objeto para tener un movimiento basado en fisica.
      */
    public void Tick()
    {
        Debug.Log("Me muevo a objetivo");
        agent.destination = objetivo.position;
        
        //Vector2 posicion = new Vector2(personaje.position.x, personaje.position.y);
        //Vector2 posicionObj = new Vector2(objetivo.position.x, objetivo.position.y);
        //Vector2 direccion = posicionObj - posicion;

        //disparos.Shoot(direccion.normalized);

    }


    

    
    public void OnEnter()
    {
        Debug.Log("Entre en Moverme");
    }

    public void OnExit()
    {
        Debug.Log("Sali moverme");
    }
}
