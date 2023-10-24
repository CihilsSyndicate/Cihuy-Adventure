using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NPCManager", menuName = "New Npc", order = 1)]
public class NPCManager : ScriptableObject
{
    public List<string> npcDialogs;
    public Sprite npcSprite;
    public string npcName;
    public string option1;
    public string option2;
    public NPCType npcType;
    public enum NPCType
    {
        normal,
        trader
    }

}