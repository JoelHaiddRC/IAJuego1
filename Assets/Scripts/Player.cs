using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    WALK,
    ATTACK,
    INTERACT,
    DAMAGED,
    PAUSED
}

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
    //Attack variables
    public float attackSpeed;
    public float knockback;
    public GameObject sword;
    public bool canAttack;
    private bool attackAvailable;
    public bool swordChar;

    //Movement variables
    public float walkSpeed;
    public bool isMoving;
    public Orientation orientation;
    Vector2 moveSpeed; //Not normalized speed
    Vector2 facingDir; 
    Vector2 lastDirection;
    Rigidbody2D rb;
    BoxCollider2D box;

    //Life variables
    public bool isAlive;
    public PlayerState currentState;
    LifeSystem lifeSystem;

    //Animation variables
    private Animator animator;
    public bool canChange;
    public GameObject swordObject;

    void Start()
    {
        lifeSystem = GetComponent<LifeSystem>();
        lifeSystem.resetState();
        isAlive = true;

        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        currentState = PlayerState.WALK;
        canChange = true;
        attackAvailable = true;

        sword.SetActive(false);

        isAlive = true;
        canAttack = true;
        facingDir = Vector2.down;
        orientation = Orientation.DOWN;

        if (attackSpeed <= 0)
        {
            Debug.LogWarning("Atack speed is 0 or below 0, its initial value will be set to 0.1");
            attackSpeed = 0.1f;
        }
        if (walkSpeed <= 0)
        {
            Debug.LogWarning("Walk speed is 0 or below 0, its initial value will be set to 0.1");
            walkSpeed = 0.1f;
        }
        if (knockback <= 0)
        {
            Debug.LogWarning("Knockback force is 0 or below 0, its initial value will be set to 0.1");
            knockback = 0.1f;
        }
    }

    void Update()
    {
        if (!isAlive)
        {
            StopWalking();
            //animator.SetTrigger("Out");
        }
        isMoving = rb.velocity != Vector2.zero;

        Inputs();
        ManageStates();
        setOrientation();
    }

    private void ManageStates()
    {
        switch (currentState)
        {
            case PlayerState.WALK:
                canAttack = true;
                Movement();
                break;
            case PlayerState.INTERACT:
                canAttack = false;
                StopWalking();
                break;
            case PlayerState.ATTACK:
                canAttack = false;
                //StopWalking();
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
            lifeSystem.DamageObject();
            lifeSystem.KnockBack(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies")
            && box.IsTouching(collision))
        {
            lifeSystem.DamageObject();
        }
    }


    private void StopWalking()
    {
        moveSpeed = Vector2.zero;
        //animator.SetBool("IsMoving", false);
    }

    public Vector2 getSpeed()
    {
        return rb.velocity;
    }

    void Inputs()
    {
        moveSpeed = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (attackAvailable && canAttack && currentState != PlayerState.ATTACK)
                StartCoroutine(Attack(attackSpeed));
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentState == PlayerState.WALK && canChange)
            {
                canChange = false;
                StartCoroutine("ChangeWeapon");
            }
        }
    }
    private IEnumerator ChangeWeapon()
    {
        currentState = PlayerState.PAUSED;
        swordChar = !swordChar;
        yield return new WaitForSeconds(0.5f);
        canChange = true;
        currentState = PlayerState.WALK;
    }


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

        /*
        animator.SetFloat("MoveX", moveSpeed.x);
        animator.SetFloat("MoveY", moveSpeed.y);
        animator.SetFloat("LastX", lastDirection.x);
        animator.SetFloat("LastY", lastDirection.y);

        if (moveSpeed.x != 0 || moveSpeed.y != 0)
            animator.SetBool("IsMoving", true);
        else
            animator.SetBool("IsMoving", false);
        */
    }

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

    private void swordPosition()
    {
        switch (orientation)
        {
            case Orientation.RIGHT:
                sword.transform.eulerAngles = new Vector3(0, 0, 90);
                swordObject.transform.localPosition = new Vector3(0.72f, 0, 0);
                swordObject.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case Orientation.LEFT:
                sword.transform.eulerAngles = new Vector3(0, 0, 270);
                swordObject.transform.localPosition = new Vector3(-0.72f, 0, 0);
                swordObject.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case Orientation.UP:
                sword.transform.eulerAngles = new Vector3(0, 0, 180);
                swordObject.transform.localPosition = new Vector3(0, 0.72f, 0);
                swordObject.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case Orientation.DOWN:
                sword.transform.eulerAngles = new Vector3(0, 0, 0);
                swordObject.transform.localPosition = new Vector3(0, -0.72f, 0);
                swordObject.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case Orientation.UPRIGHT:
                sword.transform.eulerAngles = new Vector3(0, 0, 90);
                swordObject.transform.localPosition = new Vector3(0.72f, 0, 0);
                swordObject.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case Orientation.DOWNRIGHT:
                sword.transform.eulerAngles = new Vector3(0, 0, 90);
                swordObject.transform.localPosition = new Vector3(0.72f, 0, 0);
                swordObject.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case Orientation.UPLEFT:
                sword.transform.eulerAngles = new Vector3(0, 0, 270);
                swordObject.transform.localPosition = new Vector3(-0.72f, 0, 0);
                swordObject.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case Orientation.DOWNLEFT:
                sword.transform.eulerAngles = new Vector3(0, 0, 270);
                swordObject.transform.localPosition = new Vector3(-0.72f, 0, 0);
                swordObject.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
        }

    }

    private void OnDrawGizmos()
    {
        Vector3 direction = new Vector3(facingDir.x * 2, facingDir.y * 2, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }

    private IEnumerator Attack(float reload)
    {
        canAttack = false;
        currentState = PlayerState.ATTACK;

        //animator.SetBool("IsAttacking", true);
        if (swordChar)
        {
            sword.SetActive(true);
            swordObject.SetActive(true);
            swordPosition();
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            GetComponent<Shooter>().Shoot(facingDir);
            yield return new WaitForSeconds(0.3f);
        }
        currentState = PlayerState.WALK;


        if (swordChar)
        {
            sword.SetActive(false);
            swordObject.SetActive(false);
        }
        //animator.SetBool("IsAttacking", false);

        yield return new WaitForSeconds(reload);
        canAttack = true;
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.WALK)
        {
            float speed;
            if (Input.GetKeyDown(KeyCode.LeftShift))
                speed = Time.deltaTime * walkSpeed * 2;
            else
                speed = Time.deltaTime * walkSpeed;
            rb.velocity = moveSpeed * speed;
        }
        else
        {
            if (currentState != PlayerState.DAMAGED)
                rb.velocity = Vector2.zero;
        }
    }
}
