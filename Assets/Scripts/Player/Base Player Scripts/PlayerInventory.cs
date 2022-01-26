using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject inventoryItemPrefab = null;
    [SerializeField] Transform weaponUIParent = null;
    [SerializeField] Scrollbar weaponUIScrollbar = null;

    [Header("Inventory")]
    public List<InventoryUIItem> inventory = new List<InventoryUIItem>();


    #region Public Functions
    public void SetInventory()
    {
        foreach (InventoryUIItem i in inventory)
        {
            if (i.thisInventoryType == InventoryUIItem.InventoryType.Weapon)
            {
                GameObject newWeaponCard = Instantiate(inventoryItemPrefab, weaponUIParent.position, weaponUIParent.rotation, weaponUIParent);
                newWeaponCard.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = i.ItemName;
                newWeaponCard.GetComponent<UseItem>().GetItemTypeInfo(i.thisInventoryType, i.handheldObject);
            }
        }

        weaponUIScrollbar.value = 0;
    }

    public void ClearInventory()
    {
        for (int i = 0; i < weaponUIParent.childCount; i++)
        {
            Destroy(weaponUIParent.GetChild(i).gameObject);
        }
    }

    public void AddInventory(InventoryUIItem newItem)
    {
        inventory.Add(newItem);
        List<InventoryUIItem> sortedList = inventory.OrderBy(o => o.ItemName).ToList();
        inventory = sortedList;
    }

    #endregion
}
