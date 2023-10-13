using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalInventoryItem : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryItem thisItem;
    private Collider2D itemCollider;

    private Vector3 splash;

    public float pastTime = 0;
    public float when = 0.3f;
    public Transform objectTransform;
    public float delay = 0;
    public float itemDurationOnGround = 30f;
    public bool chest = true;

    private void Awake()
    {    
         splash = new Vector3(Random.Range(-10, 10), splash.y, splash.z);
         splash = new Vector3(splash.x, Random.Range(-10, 10), splash.z);

        itemCollider = GetComponent<Collider2D>();
        itemCollider.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyAfterAWhile", itemDurationOnGround);
    }

    private void DestroyAfterAWhile()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (when >= delay)
        {
            pastTime = Time.deltaTime;
            objectTransform.position += splash * Time.deltaTime;
            delay += pastTime;         
        }
        if(delay >= when)
        {
            itemCollider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AddItemToInventory();
        }
    }

    void AddItemToInventory()
    {
        if (playerInventory && thisItem)
        {
            if (playerInventory.myInventory.Contains(thisItem))
            {
                thisItem.numberHeld += 1;
                Destroy(this.gameObject);
            }
            else
            {
                playerInventory.AddItem(thisItem, this.gameObject, thisItem);
            }
        }
    }
}
