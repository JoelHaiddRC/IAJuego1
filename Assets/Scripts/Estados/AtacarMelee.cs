using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AtacarMelee : Estado
{

    public NavMeshAgent agent;

    private GameObject ataque;

    private Animator animacion;


    public AtacarMelee(GameObject _ataque, Animator _animacion)
    {
        ataque = _ataque;
        animacion = _animacion;
    }

    public void Tick()
    {
    }
    public void OnEnter()
    {
        Debug.Log("Entre en AtacarMelee");
        animacion.SetBool("Atacar", true);
    }

    public void OnExit()
    {
        Debug.Log("Sali AtacarMelee");
        ataque.SetActive(false);
        animacion.SetBool("Atacar", false);
    }
}
