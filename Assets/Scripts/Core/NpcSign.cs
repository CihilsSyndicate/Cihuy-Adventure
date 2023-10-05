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
    public float typingSpeed = 0.05f;
    public SpriteRenderer spriteRendererToActivate;

    private int currentDialogIndex = 0;

    private bool playerInRange;
    private bool isTyping;
    private bool dialogActive = false;
    private AudioSource typingSound;
    private Text option1TextComponent;
    private Text option2TextComponent;
    private Coroutine typingCoroutine;
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
        npcSprite.sprite = npcManager.npcSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dialog()
    {
        if (!dialogActive)
        {
            ToggleDialog();
            dialogActive = true;
        }
        else if (isTyping)
        {
            // Jika teks sedang diketik, hentikan coroutine dan tampilkan seluruh teks dari dialog yang sesuai
            StopCoroutine(typingCoroutine);
            dialogText.text = npcManager.dialogs[currentDialogIndex].dialogText; // Menampilkan teks dari dialog yang sesuai
            isTyping = false;
        }
        else
        {
            // Jika teks tidak sedang diketik, atau setelah teks selesai ditampilkan
            if (currentDialogIndex < npcManager.dialogs.Count - 1)
            {
                // Lanjut ke dialog berikutnya jika masih ada dialog yang tersedia
                currentDialogIndex++;
                dialogText.text = ""; // Kosongkan teks dialog
                typingCoroutine = StartCoroutine(TypeText(currentDialogIndex)); // Mulai mengetik dialog berikutnya
            }
            else
            {
                // Tidak ada dialog lagi, tutup dialog
                dialogActive = false;
                dialogBox.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (spriteRendererToActivate != null)
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
                // Jika teks sedang diketik, hentikan coroutine dan tampilkan seluruh teks dari dialog yang sesuai
                StopCoroutine(typingCoroutine);

                // Pastikan indeks dialog saat ini valid
                if (currentDialogIndex >= 0 && currentDialogIndex < npcManager.dialogs.Count)
                {
                    dialogText.text = npcManager.dialogs[currentDialogIndex].dialogText;
                }

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
            nameNpcText.text = npcManager.npcName;

            // Memeriksa apakah indeks dialog saat ini berada dalam rentang yang benar
            if (currentDialogIndex >= 0 && currentDialogIndex < npcManager.dialogs.Count)
            {
                // Memulai coroutine dan menampilkan teks dari dialog saat ini
                typingCoroutine = StartCoroutine(TypeText(currentDialogIndex));
            }
            else
            {
                // Jika indeks dialog saat ini di luar rentang, tutup dialog
                dialogActive = false;
                dialogBox.SetActive(false);
                return;
            }

            option1TextComponent.text = npcManager.option1;
            option2TextComponent.text = npcManager.option2;
        }
    }



    IEnumerator TypeText(int dialogIndex)
    {
        isTyping = true;
        dialogText.text = "";

        string dialog = npcManager.dialogs[dialogIndex].dialogText;

        foreach (char letter in dialog)
        {
            dialogText.text += letter;

            if (typingSound != null)
            {
                typingSound.Play();
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
