using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirarIdle : Estado
{

    public Transform personaje;
    public float velocidadAngularMaxima;


    public GirarIdle(Transform _personaje, float velocidadAngularMaxima)
    {
        this.personaje = _personaje;
        this.velocidadAngularMaxima = velocidadAngularMaxima;
    }

    /**
      * Realiza el movimiento de Seek mejordado.
      * Se modifica la velocidad del objeto para tener un movimiento basado en fisica.
      */
    public void Tick()
    {
        personaje.Rotate(Vector3.forward, velocidadAngularMaxima * Time.deltaTime);
    }

    
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {

    }
    

}


