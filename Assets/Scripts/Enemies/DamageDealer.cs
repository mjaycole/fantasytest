using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool playerControlled;

    [Header("Damage Variables")]
    [SerializeField] float damage;
    [SerializeField] float damageRange;

    public float GetDamage()
    {
        return damage;
    }

    public float GetDamageRange()
    {
        return damageRange;
    }
}
