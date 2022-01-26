using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldItem : MonoBehaviour
{
    [Header("Components")]
    public string itemName;
    [SerializeField] Animator anim = null;
    [SerializeField] DamageDealer damageDealer = null;
    [SerializeField] Transform damageSphere = null;
    [SerializeField] LayerMask enemyLayer;

    [Header("Item Variables")]
    public float cooldownTime;
    [SerializeField] InventoryUIItem itemScriptableObject = null;
    [SerializeField] float shieldEffectiveness;

    [Header("FX")]
    [SerializeField] GameObject bloodSplat = null;
    

    public enum ItemTypes {
        Melee,
        Bow,
        Shield,
        Staff
    }

    public ItemTypes itemType;

    public void UseItem()
    {
        switch (itemType)
        {
            case ItemTypes.Melee:
                UseMeleeWeapon();
                break;
            case ItemTypes.Bow:
                UseBow();
                break;
            case ItemTypes.Shield:
                UseShield();
                break;
            case ItemTypes.Staff:
                UseShield();
                break;
        }
    }

    #region Private Functions

    private void UseMeleeWeapon()
    {
        anim.SetTrigger("Use");
    }

    private void UseBow()
    {

    }

    private void UseShield()
    {
        anim.SetBool("Use", true);
        GetComponent<BoxCollider>().enabled = true;
    }

    private void UseStaff()
    {

    }
    #endregion

    #region Public Functions
    public void PutAwayItem()
    {
        anim.SetTrigger("PutAway");
    }

    public void ReleaseItemUse()
    {
        anim.SetBool("Use", false);

        GetComponent<BoxCollider>().enabled = false;
    }

    public void DealDamage()
    {
        float damage = damageDealer.GetDamage();
        float damageRange = damageDealer.GetDamageRange();

        if (Physics.CheckSphere(damageSphere.position, damageRange, enemyLayer))
        {
            Collider[] enemyObjects = Physics.OverlapSphere(damageSphere.position, damageRange, enemyLayer);
            enemyObjects[0].gameObject.GetComponent<EnemyHealth>().GetDamage(damage);

            Instantiate(bloodSplat, enemyObjects[0].ClosestPoint(transform.position), Quaternion.identity);
        }
    }

    public InventoryUIItem GetItem()
    {
        return itemScriptableObject;
    }

    public float GetShieldEffectiveness()
    {
        return shieldEffectiveness;
    }
    #endregion

}
