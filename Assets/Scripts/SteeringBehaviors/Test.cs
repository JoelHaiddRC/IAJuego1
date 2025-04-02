using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public SteeringBehavior steeringBehavior;
    // Start is called before the first frame update
    void Start()
    {
        steeringBehavior = GetComponent<SteeringBehavior>();
    }

    // Para cuando se usa Translate
    void Update()
    {
        if (steeringBehavior != null)
        {
            //steeringBehavior.Execute();
        }
    }

    // Para cuando modificamos la velocidad del Rigidbody2D
    void FixedUpdate()
    {
        if (steeringBehavior != null)
        {
            steeringBehavior.Execute();
        }
    }
}
