using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shop;
    private GameObject joystick;
    // Start is called before the first frame update
    void Start()
    {
        joystick = GameObject.Find("Fixed Joystick");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement.Instance.currentState = playerState.interact;
            shop.SetActive(true);
            joystick.SetActive(false);
        }
    }

    public void CloseShop()
    {
        StartCoroutine(StopMoving());
        shop.SetActive(false);
    }

    public IEnumerator StopMoving()
    {
        PlayerMovement.Instance.fixedJoystick.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        PlayerMovement.Instance.fixedJoystick.gameObject.SetActive(true);
    }
}
