using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectile;
    public bool canShoot;
    public Vector3 spawn;
    private void Start()
    {
        if (projectile == null)
            Debug.LogWarning("There is no projectile attached to " + gameObject.name);
        canShoot = true;
    }

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
