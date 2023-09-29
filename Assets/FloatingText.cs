using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float destroyTime = 3f;
    public Vector3 offset = new Vector3(0, 2, 0);
    public Vector3 randomizeIntensity = new Vector3(0.3f, 0, 0); 

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.localPosition += offset;
        
        float randomX = Random.Range(-randomizeIntensity.x, randomizeIntensity.x);
        float randomY = Random.Range(-randomizeIntensity.y, randomizeIntensity.y);
        float randomZ = Random.Range(-randomizeIntensity.z, randomizeIntensity.z);
        transform.localPosition += new Vector3(randomX, randomY, randomZ);
    }
}
