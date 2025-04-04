using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase para objetos que puedan disparar proyectiles
public class Shooter : MonoBehaviour
{
    public GameObject projectile; //El proyectil (debe ser un prefab)
    public bool canShoot;
    public Vector3 spawn; //La posición donde aparecerá el proyectil
    
    public float waitTime;
    
    private void Start()
    {
        if (projectile == null)
            Debug.LogWarning("There is no projectile attached to " + gameObject.name);
        canShoot = true;
    }

    //Método que debe ser llamado por otra clase para disparar y en qué dirección
    public void Shoot(Vector2 direction)
    {
        if (canShoot)
        {
            canShoot = false;
            StartCoroutine("shooting", direction);
        }
    }

    private IEnumerator shooting(Vector2 direction) 
    {
        if (direction == null || direction == Vector2.zero)
        {
            Debug.LogWarning(gameObject.name + " direction not defined!");
            yield return null;
        }
        projectile.GetComponent<Projectile>().setDirection(direction);
        Instantiate(projectile, transform.position + spawn, Quaternion.identity);
        yield return new WaitForSeconds(waitTime);
        canShoot = true;
    }
}
