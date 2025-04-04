using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Torreta : MonoBehaviour
{

    
    public Transform objetivo;
    public float velocidadAngularMaxima;
    public float radioFreno;
    public float rotacionLimite;
    public GameObject jugador;
    private StateMachine _maquinaEstados;
    public float distanciaDeteccion;

    
    private void Awake()
    {

        _maquinaEstados = new StateMachine();
        var animator = GetComponent<Animator>();

        var comportamientoRotar = new Rotar(objetivo, transform, velocidadAngularMaxima, radioFreno, rotacionLimite);
        var comportamientoIdle = new GirarIdle(this.transform, velocidadAngularMaxima);
        var ataqueDistancia = new AtacarDistancia(objetivo, this.transform, GetComponent<Shooter>(), animator);

        //var comportamientoAtacar = new EstadoAtaque(objetivo, rbObjetivo, transform, GetComponent<Rigidbody2D>(), velocidadMax, distanciaParo);
        //var comportamientoHuir = new EstadoHuir(objetivo, rbObjetivo, transform, GetComponent<Rigidbody2D>(), velocidadMax, distanciaSegura);
        //var comportamientoPatrullar = new PatrullarWaypoints(waypoints, velocidadMax, distanciaCambio, transform, GetComponent<Rigidbody2D>());
        
        Func<bool> enemigoEnRango= () => Vector3.Distance(transform.position, objetivo.position) <= distanciaDeteccion;
        Func<bool> enemigoFueraDeRango = () => Vector3.Distance(transform.position, objetivo.position) > distanciaDeteccion;

        Func<bool> apuntadoObjetivo = () => {
            float diff = Vector2.Angle(transform.position, objetivo.position);
            Debug.Log(diff + " " + transform.name);
            return diff < rotacionLimite; 
        };
        
        Func<bool> noApuntadoObjetivo = () =>
        {
            float diff = Vector2.Angle(transform.position, objetivo.position);
            Debug.Log(diff + " " + transform.name);
            return diff > rotacionLimite;
        };

        


        //Func<bool> enemigoDebil = () => Vector3.Distance(transform.position, objetivo.position) < distanciaDeteccion && jugador.GetComponent<Player>().attackAvailable;
        //Func<bool> enemigoFuerte = () => Vector3.Distance(transform.position, objetivo.position) < distanciaDeteccion && !jugador.GetComponent<Player>().attackAvailable;
        //Func<bool> enemigoFueraDeRango = () => Vector3.Distance(transform.position, objetivo.position) > distanciaDeteccion;
        
        _maquinaEstados.AgregarTransicion(comportamientoIdle, comportamientoRotar, enemigoEnRango);
        _maquinaEstados.AgregarTransicion(comportamientoRotar, ataqueDistancia, apuntadoObjetivo);
        _maquinaEstados.AgregarTransicion(ataqueDistancia, comportamientoRotar, noApuntadoObjetivo);

        //_maquinaEstados.AgregarTransicion(comportamientoPatrullar, comportamientoAtacar, enemigoDebil);
        //_maquinaEstados.AgregarTransicion(comportamientoPatrullar, comportamientoHuir, enemigoFuerte);
        
        _maquinaEstados.AddAnyTransition(comportamientoIdle, enemigoFueraDeRango);
        _maquinaEstados.AsignarEstado(comportamientoRotar);
        
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _maquinaEstados.Tick();
    }
}
