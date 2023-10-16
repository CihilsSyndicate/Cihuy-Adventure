using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteract : MonoBehaviour
{
    public GameObject[] go;
    public bool trader;
    public GameObject dialogBox;
    public Text nameNpcText;
    public Text dialogText;
    public Text option1Text;
    public Text option2Text;
    private static NPCInteract instance;

    public static NPCInteract Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        instance = this;
    }

    private void OnEnable()
    {
        if (trader)
        {
            go[2].SetActive(true);
            go[3].SetActive(true);
        }
        else
        {
            go[2].SetActive(false);
            go[3].SetActive(false);
        }
    }

    public void Interact()
    {
        if (trader == true)
        {
            go[0].SetActive(false);
            go[1].SetActive(true);
        }
    }

    public void CancelShopping()
    {
        PlayerMovement.Instance.interactButton.interactable = true;
        PlayerMovement.Instance.npcSign.dialogActive = false;
        dialogBox.SetActive(false);
    }
}