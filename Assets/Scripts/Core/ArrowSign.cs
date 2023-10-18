using UnityEngine;

public class ArrowSign : MonoBehaviour
{
    private Transform player;
    private Transform target;
    public float rotationSpeed;

    private static ArrowSign instance;
    public static ArrowSign Instance
    {
        get { return instance; }
    }

    private void OnEnable()
    {
        instance = this;
    }

    private void Start()
    {       
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = FindObjectOfType<EnterNextLevel>().transform;
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y + 2.5f, player.position.x);
        // Hitung arah vektor dari tanda panah ke target
        Vector3 direction = (target.position - transform.position).normalized;

        // Hitung rotasi hanya sekitar sumbu Z menggunakan atan2
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Buat Quaternion hanya dengan rotasi sekitar sumbu Z
        Quaternion newRotation = Quaternion.Euler(0, 0, angle);

        // Terapkan rotasi secara perlahan
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }
}
