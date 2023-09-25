using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    private float speed;
    private Vector2 _direction;

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
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            other.GetComponent<SlimeController>().TakeDamage(1f);
        }
    }
}
