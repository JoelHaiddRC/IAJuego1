using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class RobotDefault : MonoBehaviour
{
    [SerializeField]
    private float velocidadMax;

    [SerializeField]
    private float distanciaSegura;

    [SerializeField]
    private float distanciaDeteccion;

    [SerializeField]
    private float distanciaParo;

    [SerializeField]
    private GameObject[] waypoints;

    [SerializeField]
    private float distanciaCambio;

    public GameObject jugador;
    public Transform objetivo;
    public Rigidbody2D rbObjetivo;

    private StateMachine _maquinaEstados;
    private Vector3 posicionInicial;

    private bool puedeAtacar() => Vector3.Distance(transform.position, objetivo.position) < distanciaDeteccion && !jugador.GetComponent<Player>().shootAvailable;

    private void Awake()
    {
        posicionInicial = transform.localPosition;
        jugador = GameObject.FindObjectOfType<Player>().gameObject;
        objetivo = jugador.transform;

        _maquinaEstados = new StateMachine();
        var animator = GetComponent<Animator>();

        var comportamientoAtacar = new EstadoAtaque(objetivo, rbObjetivo, transform, GetComponent<Rigidbody2D>(), velocidadMax, distanciaParo, animator);
        var comportamientoHuir = new EstadoHuir(objetivo, rbObjetivo, transform, GetComponent<Rigidbody2D>(), velocidadMax, distanciaSegura, animator);
        var comportamientoPatrullar = new PatrullarWaypoints(waypoints, velocidadMax, distanciaCambio, transform, GetComponent<Rigidbody2D>(), animator);
        

        


        Func<bool> enemigoDebil = () => Vector3.Distance(transform.position, objetivo.position) < distanciaDeteccion && !jugador.GetComponent<Player>().shootAvailable;
        Func<bool> enemigoFuerte = () => Vector3.Distance(transform.position, objetivo.position) < distanciaDeteccion && jugador.GetComponent<Player>().shootAvailable;
        Func<bool> enemigoFueraDeRango = () => Vector3.Distance(transform.position, objetivo.position) > distanciaDeteccion;


        _maquinaEstados.AgregarTransicion(comportamientoPatrullar, comportamientoAtacar, enemigoDebil);
        _maquinaEstados.AddAnyTransition(comportamientoHuir, enemigoFuerte);
        _maquinaEstados.AddAnyTransition(comportamientoPatrullar, enemigoFueraDeRango);
        _maquinaEstados.AsignarEstado(comportamientoPatrullar);
        
    }

    void Update()
    {
        _maquinaEstados.Tick();
    }
    private void OnDisable()
    {
        transform.localPosition = posicionInicial;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
}
