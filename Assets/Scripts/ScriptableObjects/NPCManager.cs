using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NPCManager", menuName = "New Npc", order = 1)]
public class NPCManager : ScriptableObject
{
    public Sprite npcSprite;
    public List<Dialog> dialogs; // List untuk menyimpan beberapa dialog
    public string npcName;
    public string option1;
    public string option2;
    public NPCType npcType;

    [System.Serializable]
    public class Dialog
    {
        public string dialogText;
        // Tambahkan properti lain yang mungkin Anda butuhkan, seperti suara, animasi, dll.
    }

    public enum NPCType
    {
        normal,
        trader
    }
}
