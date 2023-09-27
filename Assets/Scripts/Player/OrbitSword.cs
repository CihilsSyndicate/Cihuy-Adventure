using UnityEngine;

public class OrbitSword : Damage
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
        if (other.CompareTag("Enemy") && other.gameObject.name != "HappySlime")
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if(hit != null)
            {
                other.GetComponent<SlimeController>().Knock(hit, knockTime, damage);
            }
            Vector2 difference = hit.transform.position - transform.position;
            difference = difference.normalized * force;
            hit.AddForce(difference, ForceMode2D.Impulse);
        }

        if (other.CompareTag("Boss"))
        {
            Debug.Log("You hit the boss!");
            other.GetComponent<BossController>().TakeDamage(damage);
        }
    }
}
