using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    public float knockbackForce = 10f; // Kekuatan knockback

    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Cek apakah yang ditabrak adalah musuh
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Hitung vektor arah dari pemain ke musuh
            Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;

            // Terapkan knockback
            Knockback(knockbackDirection);
        }
    }

    private void Knockback(Vector3 knockbackDirection)
    {
        // Terapkan gaya knockback pada pemain
        playerRigidbody.velocity = Vector3.zero; // Reset kecepatan pemain
        playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }
}
