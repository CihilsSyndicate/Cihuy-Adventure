using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerState
{
    walk,
    attack,
    idle,
    stagger,
    interact
}

public class PlayerMovement : MonoBehaviour
{
    public playerState currentState;
    public float speed = 5f;
    private Animator anim;
    private Rigidbody2D myRb;
    [System.NonSerialized] public Vector3 change;
    public FixedJoystick fixedJoystick;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    private SpriteRenderer spriteRenderer;
    public float damageEffectDuration = 0.2f;
    public HealthBar healthBar;
    public GameObject floatingTextDamage;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
        anim.SetFloat("x", 0);
        anim.SetFloat("y", -1);
        healthBar.SetMaxHealth(currentHealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // change = Vector3.zero;
        change = new Vector3(fixedJoystick.Horizontal, 0f, fixedJoystick.Vertical);
        change.x = fixedJoystick.Horizontal;
        change.y = fixedJoystick.Vertical;
        change.Normalize();


        if(change == Vector3.zero)
        {
            currentState = playerState.idle;
        }

        if (Input.GetKeyDown(KeyCode.Space) && currentState != playerState.attack && currentState != playerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == playerState.walk || currentState == playerState.idle && currentState != playerState.interact)
        {
            UpdateAnimationAndMove();
        }
        else if(currentState == playerState.interact)
        {
            change = Vector3.zero;
            myRb.velocity = Vector3.zero;
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
        healthBar.SetHealth(currentHealth.RuntimeValue);
        ShowFloatingText(damage);
        StartCoroutine(DamageEffect());
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {

        }
        else
            gameObject.SetActive(false);
    }

    void ShowFloatingText(float damage)
    {
        var go = Instantiate(floatingTextDamage, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = "-" + damage.ToString();
    }

    private IEnumerator DamageEffect()
    {
        spriteRenderer.color = Color.red; // Mengubah warna menjadi merah

        yield return new WaitForSeconds(damageEffectDuration);

        spriteRenderer.color = Color.white; // Mengembalikan warna aslinya
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        healthBar.SetHealth(currentHealth.RuntimeValue);
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(knockCo(knockTime));
        }else
            gameObject.SetActive(false);

    }

    private IEnumerator knockCo(float knockTime)
    {
        if (myRb != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRb.velocity = Vector3.zero;
            currentState = playerState.idle;
            myRb.velocity = Vector3.zero;
        }
    }
}
