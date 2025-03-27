using System.Collections;
using UnityEngine;

//Clase para objetos con sistema de vida
[RequireComponent(typeof(Rigidbody2D))]
public class LifeSystem : MonoBehaviour
{
    public int life; //Vida actual
    public int maxLife; //Vida máxima
    public float invencibleTime; //Tiempo de invencibilidad al recibir un ataque
    public float thrust; //Fuerza de empuje
    public float thrustTime; //Tiempo de empuje
    private SpriteRenderer sprite;
    private Color damagedColor; //Color que se usará para hacer parpadear el sprite
    //Variables de solo lectura pública
    public bool isDamaged { get; private set; }
    public bool isDead { get; private set; }

    //Método para resetar el estado del objeto
    public void resetState()
    {
        sprite = GetComponent<SpriteRenderer>();
        isDamaged = false;
        isDead = false;
        damagedColor = sprite.color;
        life = maxLife;
    }

    //Daña el objeto y le baja un punto de vida
    //Puede modificarse con un parámetro para variar el daño
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

    //Cura el objeto subiendo un punto de vida (se puede modificar)
    public void healObject()
    {
        if(life < maxLife)
        {
            life += 1;
        }
    }

    //Genera un empuje en la dirección opuesta en la que se encuentra el hitObject
    public void KnockBack(GameObject hitObject)
    {
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();

        if (transform.CompareTag("Player"))
        {
            transform.GetComponent<Player>().currentState = PlayerState.DAMAGED;
        }
        if(transform.CompareTag("Enemies"))
        {
            transform.GetComponent<Enemy>().canChange = false;
            transform.GetComponent<Enemy>().ChangeState(EnemyState.DAMAGED);
        }
        Vector2 difference = transform.position - hitObject.transform.position;
        difference = difference.normalized * thrust;

        //Checa si hay algo que colisiona en la dirección de empuje para evitar "atascarlo"
        if (Physics2D.Raycast(transform.position, -difference, 1f, LayerMask.GetMask("Obstacles"))
            || hitObject.GetComponent<Projectile>())
        {
            Debug.Log("Can't knockback");
            if (transform.CompareTag("Enemies"))
            {
                rb.GetComponent<Enemy>().canChange = true;
                rb.GetComponent<Enemy>().ChangeState(EnemyState.IDLE);
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
    //Corutina que genera el empuje y hasta que no acabe el tiempo no pueden moverse de nuevo
    private IEnumerator KnockWait(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(thrustTime);
        rb.velocity = Vector2.zero;
        if (transform.CompareTag("Enemies"))
        {
            rb.GetComponent<Enemy>().canChange = true;
            rb.GetComponent<Enemy>().ChangeState(EnemyState.IDLE);
        }
        if (transform.CompareTag("Player"))
        {
            transform.GetComponent<Player>().currentState = PlayerState.WALK;
            transform.GetComponent<Player>().canAttack = true;
        }
    }

    //Corutina para cuando se recibe daño pero no se muere
    private IEnumerator Damage()
    {   
        isDamaged = true;
        int previousLayer = gameObject.layer;
        gameObject.layer = 12; //Layer que no colisiona con nada para evitar daño continuo
        life--;
            //Hace parpadear el sprite 4 veces mientras dura su tiempo de invencibilidad
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
        //Debug.Log("The object: " + gameObject.name + " is damaged");
    }

    //Corutina para cuando se muere
    private IEnumerator Die()
    {
        if (gameObject.tag == "Player")
            gameObject.GetComponent<Player>().isAlive = false;

        isDead = true;
        gameObject.layer = 2;
        life--;
        damagedColor.a = 0.5f;
        sprite.color = damagedColor;
        yield return new WaitForSeconds(invencibleTime);
        if(gameObject.tag != "Player")
            GameObject.Destroy(gameObject); //Los enemigos se destruyen al morir
    }
}
