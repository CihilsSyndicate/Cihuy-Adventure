using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "New Shop Item", order = 1)]
public class ShopItems : ScriptableObject
{
    public string itemName;
    public int cost;
    public float hp;
    public float atk;
    public string itemDesc;
    public Sprite itemSprite;
    public bool isOwned;
    public int consumableStack;
    public int quantity;
    [SerializeField]
    public ItemType itemType;
    public enum ItemType {
        Equipment,
        Consumable
    }
}
