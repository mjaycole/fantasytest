using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Handheld Item", menuName = "New Handheld Item")]
public class HandeheldItemScriptableObject : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
}
