using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.AI;

public class RobotKiller : MonoBehaviour
{
    [SerializeField]
    private float distanciaDeteccion;

    [SerializeField]
    private float distanciaAtaque;

    [SerializeField]
    private float areaDamage;

    [SerializeField]
    private int grafica;

    public Transform objetivo;
    public GameObject jugador;
    private StateMachine _maquinaEstados;


    private bool puedeAtacar() => Vector3.Distance(transform.position, objetivo.position) < distanciaDeteccion && !jugador.GetComponent<Player>().shootAvailable;

    private void Awake()
    {
       
        jugador = GameObject.FindObjectOfType<Player>().gameObject;
        objetivo = jugador.transform;

        _maquinaEstados = new StateMachine();
        var animator = GetComponent<Animator>();


        var comportamientoIdle = new Idle();
        var ataqueDistancia = new AtacarDistancia(objetivo, this.transform, GetComponent<Shooter>(), animator);
        var acercarseJugador = new MoverseObjetivo(objetivo, GetComponent<NavMeshAgent>(), animator);
        
        
        Func<bool> jugadorFuerte = () => jugador.GetComponent<Player>().shootAvailable;
       
        Func<bool> enemigoEnRango = () => Vector3.Distance(transform.position, objetivo.position) <= distanciaAtaque;
        Func<bool> enemigoFueraRango = () => Vector3.Distance(transform.position, objetivo.position) > distanciaAtaque;

        

        _maquinaEstados.AgregarTransicion(comportamientoIdle, acercarseJugador, jugadorFuerte);
        
        _maquinaEstados.AgregarTransicion(acercarseJugador, ataqueDistancia, enemigoEnRango);
        _maquinaEstados.AgregarTransicion(ataqueDistancia, acercarseJugador, enemigoFueraRango);


        _maquinaEstados.AsignarEstado(comportamientoIdle);
        
    }


    // Start is called before the first frame update
    void Start()
    {
        var agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        _maquinaEstados.Tick();
    }
    private void OnDisable()
    {
        
    }
}
