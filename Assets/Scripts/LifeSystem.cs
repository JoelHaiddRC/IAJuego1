using System.Collections;
using Unity.Burst.CompilerServices;
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

    public bool affectedByProjectiles; //Indica si es posible dañarlo con proyectiles
    public bool affectedByMelee; //Indica si es posible dañarlo con ataques cuerpo a cuerpo
    public bool canBeKnockbacked;

    LevelManager levelManager;

    private void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        resetState();
    }

    //Método para resetar el estado del objeto
    public void resetState()
    {
        sprite = GetComponent<SpriteRenderer>();
        isDamaged = false;
        isDead = false;
        damagedColor = sprite.color;
        life = maxLife;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "Enemies" && collision.gameObject.name.Contains("Sword"))
        {
            DamageObject(0);
            if (canBeKnockbacked)
                KnockBack(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "Enemies" && collision.gameObject.name.Contains("Sword"))
        {
            DamageObject(0);
            if (canBeKnockbacked)
                KnockBack(collision.gameObject);
        }
    }

    //Daña el objeto y le baja un punto de vida
    //El parámetro indica si es un golpe cuerpo a cuerpo (0) o de proyectil (1)
    public void DamageObject(int attackType)
    {
        if (attackType == 0 && !affectedByMelee)
            return;
        if (attackType == 1 && !affectedByProjectiles)
            return;

        if (!isDamaged && life > 0)
        {
            isDamaged = true;
            if (life > 1)
                StartCoroutine("Damage");
            else if (life == 1)
                StartCoroutine("Die");
        }
    }

    //Cura el objeto subiendo x puntos de vida
    //Si se sobrepasa la vida máxima se acota a la vida máxima
    public void healObject(int x)
    {
        life += x;
        if (life > maxLife)
            life = maxLife;
    }

    //Genera un empuje en la dirección opuesta en la que se encuentra el hitObject
    public void KnockBack(GameObject hitObject)
    {
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();

        if (transform.CompareTag("Player"))
        {
            transform.GetComponent<Player>().currentState = PlayerState.DAMAGED;
        }
        Vector2 difference = transform.position - hitObject.transform.position;
        difference = difference.normalized * thrust;

        //Checa si hay algo que colisiona en la dirección de empuje para evitar "atascarlo"
        if (Physics2D.Raycast(transform.position, -difference, 1f, LayerMask.GetMask("Obstacles"))
            || hitObject.GetComponent<Projectile>())
        {
            if (transform.CompareTag("Player"))
            {
                transform.GetComponent<Player>().currentState = PlayerState.WALK;
                transform.GetComponent<Player>().canAttack = true;
            }
        }
        else
        {
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
        if (transform.CompareTag("Player"))
        {
            transform.GetComponent<Player>().currentState = PlayerState.WALK;
            transform.GetComponent<Player>().canAttack = true;
        }
    }

    //Corutina para cuando se recibe daño pero no se muere
    private IEnumerator Damage()
    {   
        int previousLayer = gameObject.layer;
        gameObject.layer = 1; //Layer que no colisiona con nada para evitar daño continuo
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
        if (gameObject.name == "EnemyRobot")
            levelManager.killedEnemy(3);

        isDead = true;
        gameObject.layer = 2;
        life--;
        damagedColor.a = 0.5f;
        sprite.color = damagedColor;
        yield return new WaitForSeconds(invencibleTime);
        if (gameObject.tag != "Player")
        {
            if(gameObject.name.Contains("Contact"))
                levelManager.killedEnemy(0);
            if (gameObject.name.Contains("Shoot"))
                levelManager.killedEnemy(1);
            if (gameObject.name.Contains("Ray"))
                levelManager.killedEnemy(2);
            GameObject.Destroy(gameObject); //Los enemigos se destruyen al morir
        }
    }

    private void OnDisable()
    {
        if (gameObject && gameObject.CompareTag("Enemies"))
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        isDamaged = false;
        damagedColor.a = 1f;
        sprite.color = damagedColor;

    }
}
