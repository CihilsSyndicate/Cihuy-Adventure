using UnityEngine;

public class OrbitSword : MonoBehaviour
{
    public GameObject player;
    public float orbitSpeed = 10f;
    public Vector3 direction = Vector3.up;

    private void Update()
    {
        transform.RotateAround(player.transform.position, direction, orbitSpeed * Time.deltaTime);
    }
}
