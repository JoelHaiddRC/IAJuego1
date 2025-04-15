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


    public void Tick()
    {
        Debug.Log("Me muevo a objetivo");
        agent.destination = objetivo.position;

    }


    

    
    public void OnEnter()
    {
        Debug.Log("Entre en Moverme");
        agent.isStopped = false;
        animacionPersonaje.SetBool("Caminar", true);
    }

    public void OnExit()
    {
        Debug.Log("Sali moverme");
        agent.isStopped = true;
        animacionPersonaje.SetBool("Caminar", false);
    }
}
