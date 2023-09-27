using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "New Shop Item", order = 1)]
public class ShopItems : ScriptableObject
{
    public string itemName;
    public int cost;
}
