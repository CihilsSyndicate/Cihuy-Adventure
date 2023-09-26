using UnityEngine;

public class OrbitSword : MonoBehaviour
{
    public GameObject player;
    public float orbitSpeed = 10f;
    public Vector3 direction = Vector3.up;
    public float knockbackForce = 400f;

    private void Update()
    {
        transform.RotateAround(player.transform.position, direction, orbitSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {          
            other.GetComponent<SlimeController>().TakeDamage(0.1f);
        }

        if (other.CompareTag("Boss"))
        {
            Debug.Log("You hit the boss!");
            other.GetComponent<BossController>().TakeDamage(1f);
        }
    }
}
