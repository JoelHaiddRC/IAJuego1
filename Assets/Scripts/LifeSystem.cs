using System.Collections;
using UnityEngine;
public class LifeSystem : MonoBehaviour
{
    public int life;
    public int maxLife;
    public float invencibleTime;
    public float thrust;
    public float thrustTime;
    private SpriteRenderer sprite;
    private Color damagedColor;
    public bool isDamaged { get; private set; }
    public bool isDead { get; private set; }

    public void resetState()
    {
        sprite = GetSpriteRenderer();
        isDamaged = false;
        isDead = false;
        damagedColor = sprite.color;
    }

    public void DamageObject()
    {
        if (!isDamaged && life > 0)
        {
            if (life > 1)
                StartCoroutine("Damage");
            else if (life == 1)
                StartCoroutine("Die");
        }
    }

    private SpriteRenderer GetSpriteRenderer()
    {
        if (gameObject.CompareTag("Player"))
            return GetComponent<Player>().GetComponent<SpriteRenderer>();
        else
            return gameObject.GetComponent<SpriteRenderer>();
    }

    public void healObject()
    {
        if(life < maxLife)
        {
            life += 1;
            Debug.Log("Healed");
        }
    }

    public void KnockBack(GameObject hitObject)
    {
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();

        if (transform.CompareTag("Player"))
        {
            transform.GetComponent<Player>().currentState = PlayerState.DAMAGED;
        }
        if(transform.CompareTag("Enemies"))
        {
            transform.GetComponent<Enemy>().canChange = true;
            transform.GetComponent<Enemy>().currentState = EnemyState.DAMAGED;
        }
        Vector2 difference = transform.position - hitObject.transform.position;
        difference = difference.normalized * thrust;

        if (Physics2D.Raycast(transform.position, -difference, 1f, LayerMask.GetMask("Obstacles"))
            || hitObject.GetComponent<Projectile>())
        {
            Debug.Log("Can't knockback");
            if (transform.CompareTag("Enemies"))
            {
                rb.GetComponent<Enemy>().canChange = true;
                rb.GetComponent<Enemy>().currentState = EnemyState.IDLE;
            }
            if (transform.CompareTag("Player"))
            {
                transform.GetComponent<Player>().currentState = PlayerState.WALK;
                transform.GetComponent<Player>().canAttack = true;
            }
        }
        else
        {
            Debug.Log("KNOCKBACK");
            rb.velocity = Vector2.zero;
            rb.AddForce(difference, ForceMode2D.Impulse);
            StartCoroutine(KnockWait(rb));
        }
    }

    private IEnumerator KnockWait(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(thrustTime);
        rb.velocity = Vector2.zero;
        if (transform.CompareTag("Enemies"))
        {
            rb.GetComponent<Enemy>().canChange = true;
            rb.GetComponent<Enemy>().currentState = EnemyState.IDLE;
        }
        if (transform.CompareTag("Player"))
        {
            transform.GetComponent<Player>().currentState = PlayerState.WALK;
            transform.GetComponent<Player>().canAttack = true;
        }
    }

    private IEnumerator Damage()
    {   
        sprite = GetSpriteRenderer();
        isDamaged = true;
        int previousLayer = gameObject.layer;
        gameObject.layer = 12;
        life--;
            for (int i = 0; i < 4; i++)
            {
                damagedColor.a = 0.5f;
                sprite.color = damagedColor;
                yield return new WaitForSeconds(invencibleTime / 4.0f);
                damagedColor.a = 1f;
                sprite.color = damagedColor;
                yield return new WaitForSeconds(invencibleTime / 4.0f);
            }
        gameObject.layer = previousLayer;
        isDamaged = false;
        Debug.Log("The object: " + gameObject.name + " is damaged");
    }

    private IEnumerator Die()
    {
        if (gameObject.tag == "Player")
            gameObject.GetComponent<Player>().isAlive = false;

        sprite = GetSpriteRenderer();
        isDead = true;
        gameObject.layer = 2;
        life--;
        damagedColor.a = 0.5f;
        sprite.color = damagedColor;
        yield return new WaitForSeconds(invencibleTime);
        if(gameObject.tag != "Player")
            GameObject.Destroy(gameObject);
            
    }
}
