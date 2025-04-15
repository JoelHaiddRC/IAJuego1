using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.AI;

public class RobotKiller : MonoBehaviour
{
    [SerializeField]
    private float distanciaAtaqueMelee;

    [SerializeField]
    private float distanciaAtaque;

    public Transform objetivo;
    public GameObject jugador;
    public GameObject ataque;
    public GameObject escudo;
    private StateMachine _maquinaEstados;

    public LayerMask obstaculos;
    private LevelManager levelManager;
    private LifeSystem life;

    private void Awake()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        jugador = GameObject.FindObjectOfType<Player>().gameObject;
        objetivo = jugador.transform;

        _maquinaEstados = new StateMachine();
        var animator = GetComponent<Animator>();
        life = GetComponent<LifeSystem>();

        var comportamientoIdle = new Idle();
        var ataqueDistancia = new AtacarDistancia(objetivo, this.transform, GetComponent<Shooter>(), animator);
        var ataqueMelee = new AtacarMelee(ataque, animator);
        var acercarseJugador = new MoverseObjetivo(objetivo, GetComponent<NavMeshAgent>(), animator);


        Func<bool> jugadorFuerte = () => jugador.GetComponent<Player>().shootAvailable;
        Func<bool> objetivoEnVista = () => !ObstacleBetweenTarget(jugador.transform.position);
        Func<bool> objetivoFueraVista = () => ObstacleBetweenTarget(jugador.transform.position);

        Func<bool> objetivoEnAlcance = () => Vector3.Distance(transform.position, objetivo.position) < distanciaAtaqueMelee;
        Func<bool> objetivoFueraAlcance = () => Vector3.Distance(transform.position, objetivo.position) >= distanciaAtaqueMelee;

        _maquinaEstados.AgregarTransicion(comportamientoIdle, acercarseJugador, jugadorFuerte);
        
        _maquinaEstados.AgregarTransicion(acercarseJugador, ataqueDistancia, objetivoEnVista);
        _maquinaEstados.AgregarTransicion(ataqueDistancia, acercarseJugador, objetivoFueraVista);
        _maquinaEstados.AgregarTransicion(ataqueDistancia, ataqueMelee, objetivoEnAlcance);
        _maquinaEstados.AgregarTransicion(ataqueMelee, ataqueDistancia, objetivoFueraAlcance);


        _maquinaEstados.AsignarEstado(comportamientoIdle);
        
    }


    void Start()
    {
        var agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
    }

    void Update()
    {
        _maquinaEstados.Tick();
        if (levelManager.enemiesLeft <= 0)
        {
            escudo.SetActive(false);
            life.affectedByMelee = true;
        }
        else
        {
            escudo.SetActive(true);
            life.affectedByProjectiles = false;
            life.affectedByMelee = false;
        }
    }

    bool ObstacleBetweenTarget(Vector3 target)
    {
        return Physics2D.Linecast(transform.position, target, obstaculos) 
            || Vector3.Distance(transform.position, objetivo.position) >= distanciaAtaque;
    }

    private void OnDrawGizmos()
    {
        if (ObstacleBetweenTarget(jugador.transform.position))
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, jugador.transform.position);
    }
}
