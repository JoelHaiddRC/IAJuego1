using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Estados del jugador
public enum PlayerState
{
    WALK,
    ATTACK,
    DAMAGED,
    PAUSED
}

//Orientaci�n del jugador para animaciones y direcci�n de ataque
//Se definen 8 pero los diagonales se cambiar�n a uno de los primeros 4
public enum Orientation
{
    RIGHT,
    LEFT,
    UP,
    DOWN,
    UPRIGHT,
    UPLEFT,
    DOWNRIGHT,
    DOWNLEFT
}

[RequireComponent(typeof(LifeSystem))]

public class Player : MonoBehaviour
{
    //Variables de ataque
    public float attackSpeed; //Tiempo de recarga de ataque
    public float knockback;
    public GameObject sword; //El colisionador del ataque cuerpo a cuerpo
    public bool canAttack; //Variable para el tiempo de recarga del arma
    public bool attackAvailable; //Indica si ya se desbloqueo el ataque cuerpo a cuerpo
    public bool shootAvailable; //Indica si ya se desbloqueo el ataque a distancia
    public bool swordChar; //True = ataque cuerpo a cuerpo, False = ataque a distancia
    //public AudioClip swordAttackSound;

    LevelManager levelManager;

    //Variables de movimiento
    public float walkSpeed; //Velocidad m�xima
    public bool isMoving;
    public Orientation orientation;
    Vector2 moveSpeed; //Velocidad actual
    Vector2 facingDir; //Orientaci�n actual del jugador
    Vector2 lastDirection; //�ltima orientaci�n registrada 
    public Rigidbody2D rb;
    BoxCollider2D box;

    //Variables de vida
    public bool isAlive;
    public PlayerState currentState;
    LifeSystem lifeSystem;

    //Variables de animaci�n
    private Animator animator;
    public RuntimeAnimatorController animatorOrange;
    public RuntimeAnimatorController animatorRed;
    public bool canChange;

    void Start()
    {
        //Inicializar variables y obtener componentes
        lifeSystem = GetComponent<LifeSystem>();
        lifeSystem.resetState();
        isAlive = true;

        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        currentState = PlayerState.WALK;
        canChange = false;
        attackAvailable = false;
        shootAvailable = false;

        sword.SetActive(false);

        isAlive = true;
        canAttack = true;
        facingDir = Vector2.down;
        orientation = Orientation.DOWN;

        if (attackSpeed <= 0)
        {
            Debug.LogWarning("Atack speed is 0 or below 0, " +
                "its initial value will be set to 0.1");
            attackSpeed = 0.1f;
        }
        if (walkSpeed <= 0)
        {
            Debug.LogWarning("Walk speed is 0 or below 0, " +
                "its initial value will be set to 0.1");
            walkSpeed = 0.1f;
        }
        if (knockback <= 0)
        {
            Debug.LogWarning("Knockback force is 0 or below 0, " +
                "its initial value will be set to 0.1");
            knockback = 0.1f;
        }

        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        if (!isAlive)
        {
            StopWalking();
            isMoving = false;
            return;
        }

        isMoving = rb.velocity != Vector2.zero;

        Inputs();
        ManageStates();
        setOrientation();
    }

    //Indica el comportamientos de cada estado del jugador
    private void ManageStates()
    {
        switch (currentState)
        {
            case PlayerState.WALK:
                canAttack = true;
                Movement();
                break;
            case PlayerState.ATTACK:
                canAttack = false;
                StopWalking();
                break;
            case PlayerState.DAMAGED:
                canAttack = false;
                StopWalking();
                break;
            case PlayerState.PAUSED:
                canAttack = false;
                StopWalking();
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            lifeSystem.DamageObject(0);
            lifeSystem.KnockBack(collision.gameObject);
        }
    }

    //Mismo comportamiento que OnCollisionStay2D
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies")
            && box.IsTouching(collision))
        {
            lifeSystem.DamageObject(0);
        }

        if (collision.transform.name.Contains("PowerUp"))
        {
            if(!attackAvailable)
            {
                attackAvailable = true;
                levelManager.UnlockDoor();
            }
            else if (!shootAvailable)
            {
                shootAvailable = true;
                canChange = true;
            }
            StartCoroutine("PowerUpAnim");
            lifeSystem.healObject(lifeSystem.maxLife);
            Destroy(collision.transform.gameObject);
        }
    }

    private IEnumerator PowerUpAnim()
    {
        if (!shootAvailable)
            animator.SetTrigger("PowerUp1");
        else
            animator.SetTrigger("PowerUp2");
        currentState = PlayerState.PAUSED;
        yield return new WaitForSeconds(2f);
        currentState = PlayerState.WALK;

        if (!shootAvailable)
            animator.runtimeAnimatorController = animatorOrange;
        else
            animator.runtimeAnimatorController = animatorRed;
    }


    private void StopWalking()
    {
        moveSpeed = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }

    //Maneja los inputs que da el jugador
    void Inputs()
    {
        //Input de movimiento
        moveSpeed = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))    //Tevla de ataque
        {
            if (attackAvailable && canAttack && currentState != PlayerState.ATTACK)
                StartCoroutine(Attack(attackSpeed));
        }

        if (Input.GetKeyDown(KeyCode.C))    //Tecla de cambiar arma
        {
            if (currentState == PlayerState.WALK && canChange)
            {
                canChange = false;
                StartCoroutine("ChangeWeapon");
            }
        }
    }

    //Cambia el arma deteniendo el jugador y espera cierto tiempo de recarga
    private IEnumerator ChangeWeapon()
    {
        currentState = PlayerState.PAUSED;
        swordChar = !swordChar;
        yield return new WaitForSeconds(0.5f);
        canChange = true;
        currentState = PlayerState.WALK;
    }

    //Indica hacia d�nde se movi� por �ltima vez el jugador
    //para mostrar la animaci�n correcta
    private void Movement()
    {

        switch (orientation)
        {
            case Orientation.RIGHT:
                lastDirection = facingDir = Vector2.right;
                break;
            case Orientation.LEFT:
                lastDirection = facingDir = Vector2.left;
                break;
            case Orientation.UP:
                lastDirection = facingDir = Vector2.up;
                break;
            case Orientation.DOWN:
                lastDirection = facingDir = Vector2.down;
                break;
            //Los movimiento diagonales ser�n considerados como derecha o izquierda
            case Orientation.UPRIGHT:
                lastDirection = facingDir = Vector2.right;
                break;
            case Orientation.DOWNRIGHT:
                lastDirection = facingDir = Vector2.right;
                break;
            case Orientation.UPLEFT:
                lastDirection = facingDir = Vector2.left;
                break;
            case Orientation.DOWNLEFT:
                lastDirection = facingDir = Vector2.left;
                break;
        }

        //Variables para animaci�n
        animator.SetFloat("MoveX", moveSpeed.x);
        animator.SetFloat("MoveY", moveSpeed.y);
        animator.SetFloat("LastX", lastDirection.x);
        animator.SetFloat("LastY", lastDirection.y);

        if (moveSpeed.x != 0 || moveSpeed.y != 0)
            animator.SetBool("IsMoving", true);
        else
            animator.SetBool("IsMoving", false);
    }

    //Indica la orientaci�n en la que mira el jugador seg�n su movimiento actual
    private void setOrientation()
    {
        if (moveSpeed.x > 0)
        {
            if (moveSpeed.y == 0)
                orientation = Orientation.RIGHT;
            else if (moveSpeed.y > 0)
                orientation = Orientation.UPRIGHT;
            else
                orientation = Orientation.DOWNRIGHT;
        }
        else if (moveSpeed.x < 0)
        {
            if (moveSpeed.y == 0)
                orientation = Orientation.LEFT;
            else if (moveSpeed.y > 0)
                orientation = Orientation.UPLEFT;
            else
                orientation = Orientation.DOWNLEFT;
        }
        else
        {
            if (moveSpeed.y > 0)
                orientation = Orientation.UP;
            else if (moveSpeed.y < 0)
                orientation = Orientation.DOWN;
        }
    }

    //Gira la colisi�n de la espada hacia la direcci�n en la que mira el jugador
    private void swordPosition()
    {
        switch (orientation)
        {
            case Orientation.RIGHT:
                sword.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case Orientation.LEFT:
                sword.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case Orientation.UP:
                sword.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case Orientation.DOWN:
                sword.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case Orientation.UPRIGHT:
                sword.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case Orientation.DOWNRIGHT:
                sword.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case Orientation.UPLEFT:
                sword.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case Orientation.DOWNLEFT:
                sword.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
        }

    }

    //Dibuja una l�nea en la direcci�n en la que ve el jugador
    private void OnDrawGizmos()
    {
        Vector3 direction = new Vector3(facingDir.x * 2, facingDir.y * 2, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }

    //Corutina de ataque cuerpo a cuerpo y a distancia
    private IEnumerator Attack(float reload)
    {
        canAttack = false;
        currentState = PlayerState.ATTACK;

        if (swordChar)
        {
            animator.SetBool("IsAttacking", true);
            //SoundFXManager.instance.PlaySoundFXClip(swordAttackSound, transform, 1f);
            sword.SetActive(true);
            swordPosition();
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            animator.SetBool("IsShooting", true);
            GetComponent<Shooter>().Shoot(facingDir);
            yield return new WaitForSeconds(0.3f);
            animator.SetBool("IsShooting", false);
        }
        currentState = PlayerState.WALK;

        if (swordChar)
        {
            sword.SetActive(false);
            animator.SetBool("IsAttacking", false);
        }

        yield return new WaitForSeconds(reload);
        canAttack = true;
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.WALK)
        {
            float speed;
            /* Tecla para correr, opcional
            if (Input.GetKeyDown(KeyCode.LeftShift)) 
                speed = Time.deltaTime * walkSpeed * 2;
            else
            */
                speed = Time.deltaTime * walkSpeed;
            rb.velocity = moveSpeed * speed;
        }
        else
        {
            //Si el jugador es da�ado puede ser que se haya empujado
            if (currentState != PlayerState.DAMAGED)
                rb.velocity = Vector2.zero;
        }
    }
}
