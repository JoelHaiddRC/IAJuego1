using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase para objetos que puedan disparar proyectiles
public class Shooter : MonoBehaviour
{
    public GameObject projectile; //El proyectil (debe ser un prefab)
    public bool canShoot;
    public Vector3 spawn; //La posici�n donde aparecer� el proyectil
    private void Start()
    {
        if (projectile == null)
            Debug.LogWarning("There is no projectile attached to " + gameObject.name);
        canShoot = true;
    }

    //M�todo que debe ser llamado por otra clase para disparar y en qu� direcci�n
    public void Shoot(Vector2 direction)
    {
        if(direction == null || direction == Vector2.zero)
        {
            Debug.LogWarning(gameObject.name + " direction not defined!");
            return;
        }
        projectile.GetComponent<Projectile>().setDirection(direction);
        Instantiate(projectile, transform.position + spawn, Quaternion.identity);
    }
}
