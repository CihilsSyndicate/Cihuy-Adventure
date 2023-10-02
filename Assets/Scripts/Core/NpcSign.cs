using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcSign : MonoBehaviour
{
    public GameObject dialogBox;
    public Text nameNpcText;
    public Text dialogText;
    public Text option1Text; // Text untuk tombol pilihan 1
    public Text option2Text; // Text untuk tombol pilihan 2
    public string npcName;
    public string dialog;
    public string option1;
    public string option2;
    public float typingSpeed = 0.05f;
    public SpriteRenderer spriteRendererToActivate;
    private bool playerInRange;
    private bool isTyping;
    private bool dialogActive = false;
    private AudioSource typingSound;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        typingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange)
        {
            if (!dialogActive)
            {
                ToggleDialog();
                dialogActive = true;
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
            nameNpcText.text = npcName;
            StartCoroutine(TypeText());
        }
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char letter in dialog)
        {
            dialogText.text += letter;

            if (typingSound != null)
            {
                typingSound.Play();
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        // Set teks untuk dua tombol pilihan
        option1Text.text = option1;
        option2Text.text = option2;

        isTyping = false;
    }
}
