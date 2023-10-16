using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcSign : MonoBehaviour
{
    [Header("General")]
    public float typingSpeed = 0.05f;
    private bool isTyping;
    [System.NonSerialized] public bool dialogActive = false;
    private AudioSource typingSound;
    private Text option1TextComponent;
    private Text option2TextComponent;
    private Coroutine typingCoroutine;

    [Header("Dont do anything with them")]
    public GameObject dialogBox;
    public Text nameNpcText;
    public Text dialogText;
    public Text option1Text;
    public Text option2Text;

    [Header("Assignable for NPC")]
    public SpriteRenderer npcSprite;
    public NPCManager npcManager;
    public SpriteRenderer spriteRendererToActivate;

    [Header("Assignable for Chest")]
    public Chest chestDialog;
    public TreasureChest treasureChest;

    [Header("Easter Egg Makcik")]
    public InventoryItem keyItem;
    public PlayerInventory playerInventory;
    public int easterEggMakcik;
    public Makcik makcikScript;
    private SpriteRenderer itemGainedSR;

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
        dialogBox = NPCInteract.Instance.dialogBox;
        nameNpcText = NPCInteract.Instance.nameNpcText;
        dialogText = NPCInteract.Instance.dialogText;
        option1Text = NPCInteract.Instance.option1Text;
        option2Text = NPCInteract.Instance.option2Text;
        if (makcikScript != null && PlayerPrefs.GetInt("MakcikStatus") == 0)
        {
            gameObject.SetActive(true);
        }
        else if(makcikScript != null && PlayerPrefs.GetInt("MakcikStatus") == 1)
        {
            gameObject.SetActive(false);
        }

        if(makcikScript != null)
        {
            itemGainedSR = GameObject.Find("ItemGained").GetComponent<SpriteRenderer>();
        }

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
        if(easterEggMakcik == 2 && makcikScript != null && !isTyping)
        {
            npcManager.npcDialog = "Fine, you got me... He was passed away that time, so am I. " +
                "And then, this thing is for you, kid... You better keep it.";
        }
        else if(easterEggMakcik == 3 && makcikScript != null && !isTyping)
        {
            npcManager.npcName = "Secret Key";
            if (playerInventory.myInventory.Count != playerInventory.maxInventorySize)
                npcManager.npcDialog = "I hope you would listen her.";
            else
                npcManager.npcDialog = "Foolish, you should get rid of those useless things in your bag.";
        }
    }

    public void BeginDialog()
    {
        if (!dialogActive)
        {
            PlayerMovement.Instance.interactButton.interactable = false;
            ToggleDialog();
            dialogActive = true;
            if(makcikScript != null)
                easterEggMakcik += 1;
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
            PlayerMovement.Instance.interactButton.interactable = true;           
            if (gameObject.tag == "Chest")
            {
                treasureChest.itemGainedSR.enabled = false;
                PlayerMovement.Instance.anim.SetBool("Celebration", false);
            }
            if(easterEggMakcik == 3 && makcikScript != null)
            {
                BeginDialog();
                if(playerInventory.myInventory.Count != playerInventory.maxInventorySize)
                {
                    PlayerMovement.Instance.anim.SetBool("Celebration", true);
                    itemGainedSR.enabled = true;
                    itemGainedSR.sprite = keyItem.itemImage;
                    playerInventory.myInventory.Add(keyItem);
                }
                else
                {
                    PlayerMovement.Instance.anim.SetBool("Sad", true);
                }
            }
            else if (easterEggMakcik == 4 && makcikScript != null)
            {
                PlayerPrefs.SetInt("MakcikStatus", 1);
                PlayerMovement.Instance.interactButtonGO.SetActive(false);
                gameObject.SetActive(false);
                PlayerMovement.Instance.anim.SetBool("Celebration", false);
                PlayerMovement.Instance.anim.SetBool("Sad", false);
                if (itemGainedSR != null)
                    itemGainedSR.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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