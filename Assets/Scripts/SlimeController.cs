﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : MonoBehaviour
{
    [Header("Movement and Physics")]
    private Rigidbody2D rb;
    private float moveSpeed;
    private bool isMoving;
    private float interval;
    private float intervalCounter;
    private float moveDuration;
    private float moveDurationCounter;
    private Vector3 moveDirection;

    [Header("Attacking")]
    public GameObject slimeBulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", Random.Range(4f, 10f), Random.Range(9f, 11f));
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = Random.Range(1f, 2.5f);
        interval = Random.Range(1f, 3f);
        moveDuration = Random.Range(0.5f, 2f);

        intervalCounter = interval;
        moveDurationCounter = moveDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            moveDurationCounter -= Time.deltaTime;
            rb.velocity = moveDirection;

            if (moveDurationCounter < 0)
            {
                isMoving = false;
                intervalCounter = interval;
            }
        }
        else
        {
            intervalCounter -= Time.deltaTime;
            rb.velocity = Vector2.zero;

            if (intervalCounter < 0)
            {
                isMoving = true;
                moveDurationCounter = moveDuration;
                moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
            }
        }
    }

    void Shoot()
    {
        if(gameObject.tag == "Enemy")
        {
            GameObject player = GameObject.Find("Player");

            if (player != null)
            {
                GameObject bullet = Instantiate(slimeBulletPrefab);
                bullet.transform.position = transform.position;
                Vector2 direction = player.transform.position - bullet.transform.position;
                bullet.GetComponent<BulletController>().SetDirection(direction);
            }
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Test");
            Destroy(gameObject);
        }
    } */
}