using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior : MonoBehaviour
{

    public Transform objetivo;


    // Ejecucion del steering behavior deseado
    public abstract void Execute();
}
