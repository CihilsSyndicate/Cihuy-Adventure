using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerState
{
    walk,
    attack,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    public playerState currentState;
    public float speed = 5f;
    private Animator anim;
    private Rigidbody2D myRb;
    private Vector3 change;
    public FixedJoystick fixedJoystick;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;

    private static PlayerMovement instance;

    public static PlayerMovement Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
        anim.SetFloat("x", 0);
        anim.SetFloat("y", -1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // change = Vector3.zero;
        change = new Vector3(fixedJoystick.Horizontal, 0f, fixedJoystick.Vertical);
        myRb.velocity = Vector2.zero;
        change.x = fixedJoystick.Horizontal;
        change.y = fixedJoystick.Vertical;
        change.Normalize();

        //myRb.AddForce(change * speed * Time.deltaTime);

        if(change == Vector3.zero)
        {
            currentState = playerState.idle;
        }

        if (Input.GetKeyDown(KeyCode.Space) && currentState != playerState.attack)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == playerState.walk || currentState == playerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            currentState = playerState.walk;
            MoveChar();
            anim.SetFloat("x", change.x);
            anim.SetFloat("y", change.y);
            anim.SetBool("Idle", false);
        }
        else
            anim.SetBool("Idle", true);
    }

    void MoveChar()
    {
        myRb.MovePosition(
            transform.position + change.normalized * speed * Time.deltaTime
            );
    }

    private IEnumerator AttackCo()
    {
        anim.SetBool("Attack", true);
        currentState = playerState.attack;
        yield return null;
        anim.SetBool("Attack", false);
        yield return new WaitForSeconds(.3f);
        currentState = playerState.walk;
    }


    public void TakeDamage(float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {

        }
        else
            gameObject.SetActive(false);
    }
}
