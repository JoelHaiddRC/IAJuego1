using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Clase abstracta que define el comportamiento de steering
 * Se utiliza para implementar diferentes tipos de steering behaviors
 * como Seek, Flee, Arrive, etc.
 */
public abstract class SteeringBehavior : MonoBehaviour
{
    
    public Transform objetivo;

    [SerializeField]
    protected float velMaxima;

    

    // Metodo general para ejecutar el comportamiento correspondiente de steering
    public abstract void Execute();
}
