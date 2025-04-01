using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehavior : SteeringBehavior
{
    
    [SerializeField] private float speed;
    [SerializeField] private float safe_distance; 


    public override void Execute()
    {
        Debug.Log("FleeBehavior");
        Vector2 postition = new Vector2(transform.position.x, transform.position.y);
        Vector2 objetivo_pos = new Vector2(objetivo.position.x, objetivo.position.y);
        Vector2 direccion = postition - objetivo_pos;
        //direccion.z = 0;
        //Debug.DrawRay(this.transform.position, direccion, Color.yellow);
        if (direccion.magnitude < safe_distance){
            direccion = direccion.normalized * speed;
            //Vector3 velocidadDeseada = direccion;
            this.transform.Translate(direccion.x * Time.deltaTime, direccion.y * Time.deltaTime, 0.0f, Space.World);
        }
    }
    
}
