using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject pauseMenuGO;

    [Header("Movement")]
    public playerState currentState;
    public float speed = 5f;
    public Animator anim;
    public Rigidbody2D myRb;
    public FixedJoystick fixedJoystick;
    [System.NonSerialized] public Vector3 change;
    public Transform joystickHandle;
    public ParticleSystem dust;
    public AudioSource footstepSound;

    [Header("Health")]
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public Text healthBarText;
    public Text bossHealthBarText;
    public HealthBar healthBar;
    public HealthBar healthBarBig;
    public HealthBar bossHealthBar;

    [Header("Damaged")]
    public SpriteRenderer playerSpriteRenderer;
    public float damageEffectDuration = 0.2f;

    [Header("Dialog")]
    public NpcSign npcSign;
    public GameObject interactButtonGO;
    public Button interactButton;

    [Header("Inventory")]
    public PlayerInventory playerInventory;

    [Header("Game Over")]
    public GameObject popupGameOver;

    [Header("Floating Texts")]
    public GameObject floatingTextDamage;
    public GameObject floatingText;

    private static PlayerMovement instance;

    public static PlayerMovement Instance
    {
        get { return instance; }
    }

    private void OnEnable()
    {
        instance = this;
    }

    public void SavePlayer()
    {
        GameManager.Instance.SavePlayer();
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
        healthBar.SetMaxHealth(currentHealth);
        healthBarBig.SetMaxHealth(currentHealth);
        bossHealthBar.gameObject.SetActive(false);
        anim.SetFloat("x", 0);
        anim.SetFloat("y", -1);
        GameManager.Instance.LoadPlayer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthBar.SetHealth(currentHealth.RuntimeValue);
        healthBarBig.SetHealth(currentHealth.RuntimeValue);
        healthBarText.text = currentHealth.RuntimeValue.ToString() + " / " + currentHealth.maxHealth.ToString();

        if (currentHealth.RuntimeValue > currentHealth.initialValue)
        {
            currentHealth.RuntimeValue = currentHealth.initialValue;
        }

        if(currentState != playerState.stagger)
        {
            change = new Vector3(fixedJoystick.Horizontal, 0f, fixedJoystick.Vertical);
            change.x = fixedJoystick.Horizontal;
            change.y = fixedJoystick.Vertical;
            change.Normalize();
        }

        if(change == Vector3.zero && currentState != playerState.stagger)
        {
            currentState = playerState.idle;
        }
        if (currentState == playerState.walk || currentState == playerState.idle && currentState != playerState.interact && currentState != playerState.attack)
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
            if (dust != null)
                CreateDust();
            anim.SetFloat("x", change.x);
            anim.SetFloat("y", change.y);
            anim.SetBool("Celebration", false);
            anim.SetBool("Idle", false);

            if (footstepSound != null && !footstepSound.isPlaying)
            {
                footstepSound.Play();
            }
        }
        else
        {
            anim.SetBool("Idle", true);
            if (footstepSound != null && footstepSound.isPlaying)
            {
                footstepSound.Stop();
            }
        }
    }

    void MoveChar()
    {
        myRb.MovePosition(
            transform.position + change.normalized * speed * Time.deltaTime
            );
    }

    public void TakeDamage(float damage)
    {
        currentHealth.RuntimeValue -= damage;
        ShowFloatingText(damage);
        StartCoroutine(DamageEffect());
        playerHealthSignal.Raise();

        if (currentHealth.RuntimeValue <= 0)
        {
            Destroy(gameObject);
            SaveSystem.DeleteSavedData(Application.persistentDataPath + "/Player.njir");
            SaveSystem.DeleteSavedData(Application.persistentDataPath + "/BossSlime.njir");
            PlayerPrefs.DeleteAll();
            playerInventory.myInventory.Clear();
        }

        SavePlayer();
    }

    private void OnDestroy()
    {
        if (currentHealth.RuntimeValue <= 0)
        {
            Instantiate(popupGameOver);
        }
    }

    void ShowFloatingText(float damage)
    {
        var go = Instantiate(floatingTextDamage, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = "-" + damage.ToString();
    }

    public void ShowFloatingTextHp()
    {
        var go = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = "YOUR HP IS FULL!";
    }

    private IEnumerator DamageEffect()
    {
        playerSpriteRenderer.color = Color.red; // Mengubah warna menjadi merah

        yield return new WaitForSeconds(damageEffectDuration);

        playerSpriteRenderer.color = Color.white; // Mengembalikan warna aslinya
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        ShowFloatingText(damage);
        StartCoroutine(DamageEffect());
        playerHealthSignal.Raise();

        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(knockCo(knockTime));
        }
        else
        {
            Destroy(gameObject);
            SaveSystem.DeleteSavedData(Application.persistentDataPath + "/Player.njir");
            SaveSystem.DeleteSavedData(Application.persistentDataPath + "/BossSlime.njir");
            PlayerPrefs.DeleteAll();
            playerInventory.myInventory.Clear();
        }

        SavePlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Teleporter") && other.isTrigger)
        {
            StopMovement();
            this.enabled = false;
            fixedJoystick.enabled = false;
            joystickHandle.localPosition = Vector3.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("NPC")&& other.isTrigger)
        {
            NPCInteract.Instance.trader = false;
            NPCInteract.Instance.enterPoint = false;
            NPCInteract.Instance.chest = false;
            npcSign = other.GetComponent<NpcSign>();
            interactButtonGO.SetActive(true);
        }
        else if (other.CompareTag("NPC Trader") && other.isTrigger)
        {
            NPCInteract.Instance.trader = true;
            NPCInteract.Instance.enterPoint = false;
            NPCInteract.Instance.chest = false;
            npcSign = other.GetComponent<NpcSign>();
            interactButtonGO.SetActive(true);
        }
        else if (other.CompareTag("EnterPoint") && other.isTrigger)
        {
            NPCInteract.Instance.trader = false;
            NPCInteract.Instance.enterPoint = true;
            NPCInteract.Instance.chest = false;
            npcSign = other.GetComponent<NpcSign>();
            interactButtonGO.SetActive(true);
        }
        else if (other.CompareTag("Chest") && other.isTrigger)
        {
            NPCInteract.Instance.trader = false;
            NPCInteract.Instance.enterPoint = false;
            NPCInteract.Instance.chest = true;
            npcSign = other.GetComponent<NpcSign>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC Trader") || other.CompareTag("NPC") || other.CompareTag("EnterPoint"))
        {
            npcSign = null;
            interactButtonGO.SetActive(false);
        }
        if (other.CompareTag("Chest"))
        {
            npcSign = null;
        }
    }

    public void ReactivatedPlayerMovement()
    {
        this.enabled = true;
        fixedJoystick.enabled = true;
    }

    public void StopMovement()
    {
        currentState = playerState.idle;
        change = Vector3.zero;
        change.x = 0;
        change.y = 0;
        myRb.velocity = Vector3.zero;
        anim.SetBool("Idle", true);
        if (footstepSound != null && footstepSound.isPlaying)
        {
            footstepSound.Stop();
        }
    }

    public void BeginAndEndDialog()
    {
        npcSign.BeginDialog();
    }

    public void SkipDialog()
    {
        npcSign.SkipDialog();
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

    void CreateDust()
    {
        dust.Play();
    }
}
