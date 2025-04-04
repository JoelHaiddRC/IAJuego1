using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtacarDistancia : Estado
{
    
    public Transform objetivo;

    public Transform personaje;

    private Shooter disparos;

    private Animator animacionPersonaje;


    public AtacarDistancia(Transform _objetivo, Transform _personaje, Shooter _disp, Animator animacion)
    {
        disparos = _disp;
        objetivo = _objetivo;
        personaje = _personaje;
        animacionPersonaje = animacion;
    }


    /**
      * Realiza el movimiento de Seek mejordado.
      * Se modifica la velocidad del objeto para tener un movimiento basado en fisica.
      */
    public void Tick()
    {
        
        
        
        Vector2 posicion = new Vector2(personaje.position.x, personaje.position.y);
        Vector2 posicionObj = new Vector2(objetivo.position.x, objetivo.position.y);
        Vector2 direccion = posicionObj - posicion;

        disparos.Shoot(direccion.normalized);

    }


    

    
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {

    }
}
