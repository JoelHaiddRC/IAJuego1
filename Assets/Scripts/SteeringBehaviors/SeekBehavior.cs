using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBehavior : SteeringBehavior
{
    [SerializeField] private float speed;
    [SerializeField] private float stop_distance; 


    public override void Execute()
    {
        
        Debug.Log("SeekBehavior");
        if (objetivo != null){
            Vector2 direccion = new Vector2(objetivo.position.x, objetivo.position.y) - new Vector2(this.transform.position.x, this.transform.position.y);
            //Debug.DrawRay(this.transform.position, direccion, Color.red);
            //direccion.y = 0;
            if (direccion.magnitude > stop_distance){
                direccion = direccion.normalized * speed;
                //transform.position += velocidad * Time.deltaTime;
                this.transform.Translate(direccion.x * Time.deltaTime, direccion.y * Time.deltaTime, 0.0f, Space.World);
            }
        }
    }
    
    
}
