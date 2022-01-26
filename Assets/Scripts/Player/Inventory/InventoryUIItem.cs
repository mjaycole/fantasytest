using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UI Item", menuName = "New UI Item")]
public class InventoryUIItem : ScriptableObject
{
    public enum InventoryType
    {
        Weapon,
        Armor,
        Treasure
    }

    public InventoryType thisInventoryType;

    public string ItemName;
    public HandeheldItemScriptableObject handheldObject;
}
