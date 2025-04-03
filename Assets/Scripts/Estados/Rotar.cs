using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotar : Estado
{

    
    public Transform objetivo;
    public Transform personaje;
    public float velocidadAngularMaxima;
    public float radioFreno;
    public float rotacionLimite;


    public Rotar(Transform _objetivo, Transform _personaje, float velocidadAngularMaxima, float radioFreno, float rotacionLimite)
    {
        this.objetivo = _objetivo;
        this.personaje = _personaje;
        this.velocidadAngularMaxima = velocidadAngularMaxima;
        this.radioFreno = radioFreno;
        this.rotacionLimite = rotacionLimite;
    }

    /**
      * Realiza el movimiento de Seek mejordado.
      * Se modifica la velocidad del objeto para tener un movimiento basado en fisica.
      */
    public void Tick()
    {
        Align2D();
    }

    
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {

    }
    

    private void Align() 
    {
        if (objetivo != null) {
            Vector3 direccion = objetivo.position - personaje.position;
            direccion.z = 0;

            if (direccion.sqrMagnitude > 0.001f) {
                Quaternion rotationObjetivo = Quaternion.LookRotation(direccion);

                float anguloDiferencia = Quaternion.Angle(personaje.rotation, rotationObjetivo);

                if (anguloDiferencia > rotacionLimite) {
                    float velocidadAngular = velocidadAngularMaxima;
                    if (anguloDiferencia < radioFreno){
                        velocidadAngular = anguloDiferencia / radioFreno;
                    }
                    personaje.rotation = Quaternion.RotateTowards(personaje.rotation, rotationObjetivo, velocidadAngular * Time.deltaTime);

                    //Debug.DrawRay(transform.position, transform.forward * 2f, Color.yellow);
                    //Debug.DrawRay(transform.position, direccion.normalized * 2f, Color.green);
                }
            }
        }
    }

    private void Align2D() {
        if (objetivo != null) {
            Vector2 posicion = new Vector2(personaje.position.x, personaje.position.y);
            Vector2 posicionObj = new Vector2(objetivo.position.x, objetivo.position.y);
            
            Vector2 direccion =  posicion - posicionObj;
            if (direccion.sqrMagnitude > 0.001f) {
                float anguloObjetivo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
                float anguloActual = personaje.eulerAngles.z + 270f;
                Debug.Log( anguloObjetivo);
                float anguloDiferencia = Mathf.DeltaAngle(anguloActual, anguloObjetivo);
                
                if (Mathf.Abs(anguloDiferencia) > rotacionLimite) {
                    float velocidadAngular = velocidadAngularMaxima;
                    if (Mathf.Abs(anguloDiferencia) < radioFreno){
                        velocidadAngular = Mathf.Abs(anguloDiferencia) / radioFreno;
                    }
                    float nuevoAngulo = Mathf.MoveTowardsAngle(anguloActual, anguloObjetivo, velocidadAngular * Time.deltaTime);
                    Quaternion rotacionObjetivo = Quaternion.Euler(0, 0, anguloObjetivo);
                    personaje.rotation = Quaternion.RotateTowards(personaje.rotation, rotacionObjetivo, velocidadAngularMaxima * Time.deltaTime);
                    //personaje.rotation = Quaternion.Euler(0, 0, nuevoAngulo);

                }
            }
        }
    }

}


