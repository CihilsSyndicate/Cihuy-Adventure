using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    private float speed;
    private Vector2 _direction;
    public float damage = 5f;

    private void Awake()
    {
        speed = 5f;
    }

    private void Update()
    {
        Vector2 position = transform.position;
        position += _direction * speed * Time.deltaTime;
        transform.position = position;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && other.gameObject.name != "HappySlime")
        {
            other.GetComponent<SlimeController>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.CompareTag("Boss"))
        {
            other.GetComponent<BossController>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            Debug.Log("Njir");
            Destroy(gameObject);
        }
    }
}
