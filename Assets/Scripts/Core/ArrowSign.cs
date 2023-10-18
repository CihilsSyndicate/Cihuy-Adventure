using UnityEngine;

public class ArrowSign : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed;

    private void Update()
    {
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
