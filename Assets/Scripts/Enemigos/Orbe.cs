using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbe : MonoBehaviour
{

    
    public Transform objetivo;
    public float velocidadAngularMaxima;
    public float radioFreno;
    public float rotacionLimite;
    public GameObject jugador;
    private StateMachine _maquinaEstados;
    public float distanciaDeteccion;
    private Quaternion rotacionInicial;

    
    private void Awake()
    {
        jugador = GameObject.FindObjectOfType<Player>().gameObject;
        objetivo = jugador.transform;
        _maquinaEstados = new StateMachine();
        var animator = GetComponent<Animator>();

        var comportamientoRotar = new Rotar(objetivo, transform, velocidadAngularMaxima, radioFreno, rotacionLimite);
        var estadoIdle = new GirarIdle(this.transform, velocidadAngularMaxima);



        //var comportamientoAtacar = new EstadoAtaque(objetivo, rbObjetivo, transform, GetComponent<Rigidbody2D>(), velocidadMax, distanciaParo);
        //var comportamientoHuir = new EstadoHuir(objetivo, rbObjetivo, transform, GetComponent<Rigidbody2D>(), velocidadMax, distanciaSegura);
        //var comportamientoPatrullar = new PatrullarWaypoints(waypoints, velocidadMax, distanciaCambio, transform, GetComponent<Rigidbody2D>());
        Func<bool> enemigoEnRango = () => Vector3.Distance(transform.position, objetivo.position) <= distanciaDeteccion;
        Func<bool> enemigoFueraDeRango = () => Vector3.Distance(transform.position, objetivo.position) > distanciaDeteccion;

        

        _maquinaEstados.AgregarTransicion(estadoIdle, comportamientoRotar, enemigoEnRango);

        _maquinaEstados.AddAnyTransition(estadoIdle, enemigoFueraDeRango);

        //Func<bool> enemigoDebil = () => Vector3.Distance(transform.position, objetivo.position) < distanciaDeteccion && jugador.GetComponent<Player>().attackAvailable;
        //Func<bool> enemigoFuerte = () => Vector3.Distance(transform.position, objetivo.position) < distanciaDeteccion && !jugador.GetComponent<Player>().attackAvailable;
        //Func<bool> enemigoFueraDeRango = () => Vector3.Distance(transform.position, objetivo.position) > distanciaDeteccion;


        //_maquinaEstados.AgregarTransicion(comportamientoPatrullar, comportamientoAtacar, enemigoDebil);
        //_maquinaEstados.AgregarTransicion(comportamientoPatrullar, comportamientoHuir, enemigoFuerte);
        //_maquinaEstados.AddAnyTransition(comportamientoPatrullar, enemigoFueraDeRango);
        _maquinaEstados.AsignarEstado(estadoIdle);
        
    }



    // Start is called before the first frame update
    void Start()
    {
        rotacionInicial = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        _maquinaEstados.Tick();
    }

    private void OnDisable()
    {
        transform.rotation = rotacionInicial;
    }
}
