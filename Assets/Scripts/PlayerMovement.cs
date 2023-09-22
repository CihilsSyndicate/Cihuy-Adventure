using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Animator anim;
    private Rigidbody2D myRb;
    private Vector3 change;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        UpdateAnimationAndMove();
        
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
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
}
