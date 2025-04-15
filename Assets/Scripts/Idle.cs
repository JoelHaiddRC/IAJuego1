using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Estado
{


    public Idle()
    {
        
    }

    /**
      * Realiza el movimiento de Seek mejordado.
      * Se modifica la velocidad del objeto para tener un movimiento basado en fisica.
      */
    public void Tick()
    {
        
    }

    
    public void OnEnter()
    {
        Debug.Log("Entre en Idle");
    }

    public void OnExit()
    {
      Debug.Log("Sali en Idle");
    }
    

}


