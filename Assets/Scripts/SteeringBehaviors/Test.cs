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

    // Update is called once per frame
    void Update()
    {
        if (steeringBehavior != null)
        {
            steeringBehavior.Execute();
        }
    }
}
