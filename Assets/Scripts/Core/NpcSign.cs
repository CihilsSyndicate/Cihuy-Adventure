﻿using System.Collections;
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
    private Text option1TextComponent;
    private Text option2TextComponent;
    private Coroutine typingCoroutine; 

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        typingSound = GetComponent<AudioSource>();

        option1TextComponent = option1Text.GetComponent<Text>();
        option2TextComponent = option2Text.GetComponent<Text>();
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
            else if (isTyping)
            {
                // Jika teks sedang diketik, hentikan coroutine dan tampilkan seluruh teks
                StopCoroutine(typingCoroutine);
                dialogText.text = dialog;
                isTyping = false;
            }
            else
            {
              
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
                StopCoroutine(typingCoroutine);
                dialogText.text = dialog;
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
            nameNpcText.text = npcName;

            // Mulai coroutine dan simpan referensi ke coroutine saat ini
            typingCoroutine = StartCoroutine(TypeText());

            option1TextComponent.text = option1;
            option2TextComponent.text = option2;
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

        isTyping = false;
    }
}
