using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveBehavior : SteeringBehavior
{


    
    [SerializeField] private float speed;
    [SerializeField] private float slow_radius;
    [SerializeField] private float stop_radius;


    public override void Execute()
    {
        
        Debug.Log("ArriveBehavior");

        if (objetivo != null) {
            Vector2 direccion = new Vector2(objetivo.position.x, objetivo.position.y) - new Vector2(this.transform.position.x, this.transform.position.y);
            
            float distanceToTarget = direccion.magnitude;
            //Debug.DrawRay(transform.position, direccion, Color.blue);

            if( distanceToTarget > stop_radius) {
                float actual_speed = speed;

                if (distanceToTarget < slow_radius) {
                    actual_speed = speed * (distanceToTarget / slow_radius);
                }
                direccion = direccion.normalized * actual_speed;
                this.transform.Translate(direccion.x * Time.deltaTime, direccion.y * Time.deltaTime, 0.0f, Space.World);


            }
        }
    }
    
    
}
