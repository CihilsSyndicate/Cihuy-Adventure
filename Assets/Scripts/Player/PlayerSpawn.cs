using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private Transform player;
    public string fromWhichScene;
    // Start is called before the first frame update
    void Start()
    {
        if(fromWhichScene == PlayerPrefs.GetString("SpawnPoint"))
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            player.position = transform.position;        
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
