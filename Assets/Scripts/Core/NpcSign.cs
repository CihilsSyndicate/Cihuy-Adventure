using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcSign : MonoBehaviour
{
    [Header("General")]
    private int currentDialogIndex = 0;
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
    public NPCManager dialogSecretKey;
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
        if(gameObject.tag != "Chest" && npcSprite != null)
            npcSprite.sprite = npcManager.npcSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(easterEggMakcik == 10 && makcikScript != null && !isTyping)
        {
            npcManager.npcDialogs[0] = "Fine, you got me... He was passed away that time, so am I. " +
                "And then, this thing is for you, kid... You better keep it.";
        }
        else if(easterEggMakcik == 11 && makcikScript != null && !isTyping)
        {
            npcManager.npcName = "Secret Key";
            if (playerInventory.myInventory.Count != playerInventory.maxInventorySize)
                npcManager.npcDialogs[0] = "I hope you would listen her.";
            else
                npcManager.npcDialogs[0] = "Foolish, you should get rid of those useless things in your bag.";
        }
    }

    public void BeginDialog()
    {
        if (!dialogActive)
        {
            PlayerMovement.Instance.interactButton.interactable = false;
            currentDialogIndex = 0;
            ToggleDialog();
            dialogActive = true;
            if (makcikScript != null) {
                easterEggMakcik += 1;
            }
        }
    }

    public void SkipDialog()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            if(gameObject.tag != "Chest")
            {
                dialogText.text = GetCurrentDialog();
            }
            else
            {
                dialogText.text = chestDialog.dialog;
            }
            isTyping = false;
        }
        else if (dialogActive && !isTyping && !gameObject.CompareTag("NPC Trader") && !gameObject.CompareTag("EnterPoint"))
        {         
            if (gameObject.tag == "Chest")
            {
                treasureChest.itemGainedSR.enabled = false;
                PlayerMovement.Instance.anim.SetBool("Celebration", false);
            }
            if(easterEggMakcik == 11 && makcikScript != null)
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
            else if (easterEggMakcik == 12 && makcikScript != null)
            {
                npcManager.npcName = "Mrs. Siti";
                npcManager.npcDialogs[0] = "...";
                PlayerPrefs.SetInt("MakcikStatus", 1);
                PlayerMovement.Instance.interactButtonGO.SetActive(false);               
                PlayerMovement.Instance.anim.SetBool("Celebration", false);
                PlayerMovement.Instance.anim.SetBool("Sad", false);
                if (itemGainedSR != null)
                    itemGainedSR.enabled = false;
            }

            if (currentDialogIndex < npcManager.npcDialogs.Count - 1)
            {
                currentDialogIndex++;
                ToggleDialog();
            }
            else
            {
                dialogActive = false;
                dialogBox.SetActive(false);
                PlayerMovement.Instance.interactButton.interactable = true;
            }
        }
    }

    private string GetCurrentDialog()
    {
        if (currentDialogIndex < npcManager.npcDialogs.Count)
        {
            return npcManager.npcDialogs[currentDialogIndex];
        }
        return "";
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
            if (spriteRendererToActivate != null)
            {
                spriteRendererToActivate.enabled = false;
            }
        }
    }

    private void ToggleDialog()
    {
        if (dialogBox.activeInHierarchy && currentDialogIndex >= npcManager.npcDialogs.Count)
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

            dialogText.text = GetCurrentDialog();
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
            string currentDialog = GetCurrentDialog(); // Dapatkan dialog saat ini
            foreach (char letter in currentDialog)
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
            string currentDialog = GetCurrentDialog(); // Dapatkan dialog saat ini
            foreach (char letter in currentDialog)
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