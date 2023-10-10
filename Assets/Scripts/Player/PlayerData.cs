using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public float health;
    public float[] position;
    public InventoryItem currentItem;

    public PlayerData (PlayerMovement player)
    {
        health = player.currentHealth.RuntimeValue;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        currentItem = WeaponManager.Instance.weapon;
    }
}
