using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinchos : MonoBehaviour
{
    public float tiempoRecarga;
    public float tiempoAtaque;
    public Animator animatorCelda;
    public Animator animatorPincho;
    bool canAttack;
    public BoxCollider2D pinchoCollider;

    void Start()
    {
        canAttack = true;
        pinchoCollider.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.transform.tag == "Player" || collision.gameObject.layer == 6) && canAttack)
        {
            canAttack = false;
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        animatorCelda.SetTrigger("Charge");
        yield return new WaitForSeconds(tiempoAtaque);
        animatorPincho.SetTrigger("Attack");
        yield return new WaitForSeconds(.5f);
        animatorCelda.SetTrigger("Attack");
        pinchoCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        pinchoCollider.enabled = false;
        animatorPincho.SetTrigger("Idle");
        yield return new WaitForSeconds(tiempoRecarga);
        animatorCelda.SetTrigger("Idle");
        canAttack = true;
    }

    private void OnDisable()
    {
        pinchoCollider.enabled = false;
        canAttack = true;
    }
}
