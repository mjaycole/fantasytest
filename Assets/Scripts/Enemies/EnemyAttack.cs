using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] EnemyAI enemyAI = null;

    public void DealDamage()
    {
        enemyAI.DealDamage();
    }
}
