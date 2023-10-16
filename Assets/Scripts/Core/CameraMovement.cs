using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;
    public float smoothing;
    public Vector2 maxPos;
    public Vector2 minPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player.position != transform.position)
        {
            Vector3 playerPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

            playerPosition.x = Mathf.Clamp(playerPosition.x, minPos.x, maxPos.x);
            playerPosition.y = Mathf.Clamp(playerPosition.y, minPos.y, maxPos.y);

            transform.position = Vector3.Lerp(transform.position, playerPosition, smoothing);
        }
        
        
    }
}
