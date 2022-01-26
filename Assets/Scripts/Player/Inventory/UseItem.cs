using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public InventoryUIItem.InventoryType itemType;
    public HandeheldItemScriptableObject handheldItemScript = null;

    [Header("Components")]
    [SerializeField] PlayerItemsInHand itemsScript = null;

    private void Start()
    {
        itemsScript = transform.root.GetComponent<PlayerItemsInHand>();
    }

    #region Public Functions
    public void GetItemTypeInfo(InventoryUIItem.InventoryType newType, HandeheldItemScriptableObject isHandheld = null)
    {
        itemType = newType;
        handheldItemScript = isHandheld;
    }

    public void AttemptUseItem()
    {
        switch (itemType)
        {
            case InventoryUIItem.InventoryType.Weapon:
                itemsScript.SetRightHandItem(handheldItemScript);
                break;
        }
    }

    #endregion
}
