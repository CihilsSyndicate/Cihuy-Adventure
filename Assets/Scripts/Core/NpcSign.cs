using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcSign : MonoBehaviour
{
    public GameObject dialogBox;
    public Text nameNpcText;
    public Text dialogText;
    public Text option1Text;
    public Text option2Text;
    public SpriteRenderer npcSprite;

    public NPCManager npcManager;
    public Chest chestDialog;
    public TreasureChest treasureChest;
    public float typingSpeed = 0.05f;
    public SpriteRenderer spriteRendererToActivate;

    private bool playerInRange;
    private bool isTyping;
    private bool dialogActive = false;
    private AudioSource typingSound;
    private Text option1TextComponent;
    private Text option2TextComponent;
    private Coroutine typingCoroutine;

    [System.NonSerialized] public int easterEggMakcik;

    private static NpcSign instance;
    public static NpcSign Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        typingSound = GetComponent<AudioSource>();

        option1TextComponent = option1Text.GetComponent<Text>();
        option2TextComponent = option2Text.GetComponent<Text>();
        if(gameObject.tag != "Chest")
            npcSprite.sprite = npcManager.npcSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BeginDialog()
    {
        if (!dialogActive)
        {
            PlayerMovement.Instance.interactButtonGO.gameObject.SetActive(false);
            ToggleDialog();
            dialogActive = true;           
        }
    }

    public void SkipDialog()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            if(gameObject.tag != "Chest")
            {
                dialogText.text = npcManager.npcDialog;
            }
            else
            {
                dialogText.text = chestDialog.dialog;
            }
            isTyping = false;
        }
        else if (dialogActive && !isTyping && !gameObject.CompareTag("NPC Trader"))
        {
            dialogActive = false;
            dialogBox.SetActive(false);

            if(gameObject.tag == "Chest")
            {
                treasureChest.itemGainedSR.enabled = false;
                PlayerMovement.Instance.anim.SetBool("Celebration", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (spriteRendererToActivate != null && gameObject.tag != "Chest")
            {
                spriteRendererToActivate.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // Hentikan teks yang sedang ditampilkan saat keluar dari jangkauan
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogText.text = npcManager.npcDialog;
                isTyping = false;
            }

            dialogActive = false;
            dialogBox.SetActive(false);

            if (spriteRendererToActivate != null)
            {
                spriteRendererToActivate.enabled = false;
            }
        }
    }

    void ToggleDialog()
    {
        if (dialogBox.activeInHierarchy)
        {
            dialogBox.SetActive(false);
        }
        else
        {
            dialogBox.SetActive(true);
            if(gameObject.tag != "Chest")
            {
                nameNpcText.text = npcManager.npcName;
            }
            else
            {
                nameNpcText.text = "Rare Chest";
            }

            typingCoroutine = StartCoroutine(TypeText());

            if(gameObject.tag != "Chest")
            {
                option1TextComponent.text = npcManager.option1;
                option2TextComponent.text = npcManager.option2;
            }
        }
    }


    IEnumerator TypeText()
    {
        isTyping = true;
        dialogText.text = "";

        if (gameObject.tag != "Chest")
        {
            foreach (char letter in npcManager.npcDialog)
            {
                dialogText.text += letter;

                if (typingSound != null)
                {
                    typingSound.Play();
                }

                yield return new WaitForSeconds(typingSpeed);
            }
        }
        else
        {
            foreach (char letter in chestDialog.dialog)
            {
                dialogText.text += letter;

                if (typingSound != null)
                {
                    typingSound.Play();
                }

                yield return new WaitForSeconds(typingSpeed);
            }
        }

        isTyping = false;
    }
}