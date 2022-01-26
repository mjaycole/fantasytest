using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator anim = null;
    [SerializeField] EnemyAI enemyAI = null;
    [SerializeField] Transform weaponParent = null;
    [SerializeField] NavMeshAgent agent = null;

    [Header("Health Variables")]
    [SerializeField] float currentMaxHealth;
    [SerializeField] float currentHealth;

    public UnityEvent onEnemyDeath;

    private void Start()
    {
        currentHealth = currentMaxHealth;
    }

    #region Private Functions
    private void DisableNecessaryThingsOnDeath()
    {
        anim.applyRootMotion = true;
        enemyAI.enabled = false;
        weaponParent.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
        weaponParent.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
        weaponParent.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        weaponParent.GetChild(0).transform.parent = null;
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
    #endregion


    #region Public Functions
    public void GetDamage(float amount)
    {
        currentHealth -= amount;

        //Aggro enemy if hit
        enemyAI.BecomeAggro();
        

        if (currentHealth <= 0)
        {
            anim.SetTrigger("Die");
            DisableNecessaryThingsOnDeath();
            onEnemyDeath.Invoke();
        }
    }
    #endregion
}
