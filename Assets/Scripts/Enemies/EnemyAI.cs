using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Investigate,
        Chase,
        Attack,
        Flee
    }

    public State state;

    #region Variables
    [Header("Components")]
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Animator anim = null;
    [SerializeField] Transform player = null;
    [SerializeField] Transform enemyWeaponParent = null;
    DamageDealer enemyWeapon = null;
    [SerializeField] Transform damageSphere = null;

    [Header("Movement Variables")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float timeAtPatrolPoint;
    float idleTimer;
    [SerializeField] Transform testTarget = null;
    [SerializeField] Transform patrolPointParent = null;
    int patrolPointIndex;
    public Transform nextPatrolPoint = null;

    [Header("State Machine")]
    [SerializeField] bool hostile = true;
    [SerializeField] float hostileTime;
    public float hostileTimer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask shieldLayer;
    [SerializeField] LayerMask enemyLayer;
    public float sightDistance;
    [Range(0, 360)]
    public float fovAngle;
    public bool seePlayer = false;
    [SerializeField] float alertOtherEnemiesRadius;
    bool canAttack = true;
    bool startedAttackCycle;
    bool attacking;
    bool patrolling;
    bool chasing;
    bool idle;

    [Header("Attacking Variables")]
    [SerializeField] float attackRange;
    [SerializeField] float timeBetweenAttacks;

    [Header("FX")]
    [SerializeField] GameObject bloodSplat = null;
    [SerializeField] GameObject shieldHit = null;

    #endregion

    #region Start and Update
    private void Start()
    {
        agent.speed = walkSpeed;
        player = GameObject.FindObjectOfType<PlayerMovement>().transform;
        patrolPointParent.parent = null;
        enemyWeapon = enemyWeaponParent.GetComponentInChildren<DamageDealer>();
    }

    private void Update()
    {
        StateMachine();
        StateSwitch();
        CheckSightOfPlayer();
    }
    #endregion

    #region Private Functions
    private void StateMachine()
    {
        switch (state)
        {
            case State.Idle:
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                chasing = false;
                attacking = false;
                patrolling = false;

                HandleIdleState();
                break;
            case State.Patrol:
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                chasing = false;
                attacking = false;
                patrolling = true;
                idle = false;

                HandlePatrolState();
                break;
            case State.Investigate:
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                break;
            case State.Chase:
                anim.SetBool("Walk", false);
                anim.SetBool("Run", true);
                chasing = true;
                idle = false;
                patrolling = false;
                attacking = false;

                HandleChaseState();
                break;
            case State.Attack:
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                chasing = false;
                patrolling = false;
                idle = false;

                if (!startedAttackCycle && canAttack)
                {
                    HandleAttackState();
                }
                break;
            case State.Flee:
                anim.SetBool("Walk", false);
                anim.SetBool("Run", true);
                break;
        }
    }

    private void StateSwitch()
    {
        if (seePlayer)
        {
            if (hostile)
            {
                if (!attacking)
                {
                    if (Vector3.Distance(player.position, transform.position) > attackRange)
                    {
                        state = State.Chase;
                        hostileTimer = hostileTime;
                    }
                    else if (canAttack)
                    {
                        state = State.Attack;
                    }
                }
                else
                {
                    if (!canAttack && !startedAttackCycle)
                    {
                        state = State.Chase;
                    }
                }
            }
        }
        else
        {
            if (patrolling)
            {
                state = State.Patrol;
            }
            else if (chasing)
            {
                hostileTimer -= Time.deltaTime;

                if (hostileTimer <= 0)
                {
                    state = State.Idle;
                }
            }
            else if (attacking)
            {
                if (!canAttack)
                {
                    state = State.Chase;
                }

                else
                {
                    state = State.Attack;
                }
            }
            else
            {
                state = State.Idle;
            }
        }
    }

    private void CheckSightOfPlayer()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, sightDistance, playerLayer);
        {
            if (rangeChecks.Length != 0)
            {
                Vector3 directionToPlayer = (new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToPlayer) < fovAngle / 2)
                {
                    float distance = Vector3.Distance(transform.position, player.position);

                    if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), directionToPlayer, distance, groundLayer))
                    {
                        seePlayer = true;
                    }
                    else
                    {
                        seePlayer = false;
                    }
                }
                else
                {
                    seePlayer = false;
                }
            }
            else
            {
                seePlayer = false;
            }
        }
    }
   

    private void HandleIdleState()
    {
        agent.SetDestination(transform.position);
        agent.speed = walkSpeed;
        idleTimer -= Time.deltaTime;

        if (!idle)
        {
            idle = true;
            idleTimer = timeAtPatrolPoint;
        }

        if (idleTimer <= 0)
        {
            if (patrolPointParent.childCount != 0)
            {
                patrolling = true;
                idle = false;
            }
        }
    }

    private void HandlePatrolState()
    {
        if (nextPatrolPoint == null)
        {
            if (patrolPointParent.childCount - 1 > patrolPointIndex)
            {
                patrolPointIndex++;
                nextPatrolPoint = patrolPointParent.GetChild(patrolPointIndex);
            }
            else
            {
                patrolPointIndex = 0;
                nextPatrolPoint = patrolPointParent.GetChild(patrolPointIndex);
            }
        }
        else
        {
            agent.SetDestination(nextPatrolPoint.position);
            agent.speed = walkSpeed;

            if (Vector3.Distance(transform.position, nextPatrolPoint.position) < agent.stoppingDistance + .01f)
            {
                patrolling = false;
                nextPatrolPoint = null;
            }
        }
    }

    private void HandleChaseState()
    {
        agent.SetDestination(player.position);
        agent.speed = runSpeed;

        AlertNearbyEnemiesWhenAggro();
    }

    private void HandleAttackState()
    {
        StartCoroutine(InitiateAttack());
    }

    private void AlertNearbyEnemiesWhenAggro()
    {
        if (Physics.CheckSphere(transform.position, alertOtherEnemiesRadius, enemyLayer))
        {
            foreach (Collider c in Physics.OverlapSphere(transform.position, alertOtherEnemiesRadius, enemyLayer))
            {
                c.GetComponent<EnemyAI>().BecomeAggro();
            }
        }
    }

    IEnumerator InitiateAttack()
    {
        anim.SetTrigger("Attack");
        agent.SetDestination(transform.position);

        startedAttackCycle = true;
        canAttack = false;
        attacking = true;

        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);

        attacking = false;
        startedAttackCycle = false;

        StartCoroutine(TimeBetweenAttacks());
    }

    IEnumerator TimeBetweenAttacks()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        canAttack = true;
    }

    #endregion

    #region Public Functions
    public void DealDamage()
    {
        float damage = enemyWeapon.GetDamage();
        float damageRange = enemyWeapon.GetDamageRange();

        if (Physics.CheckSphere(damageSphere.position, damageRange, shieldLayer))
        {
            Collider[] shield = Physics.OverlapSphere(damageSphere.position, damageRange, shieldLayer);
            Instantiate(shieldHit, shield[0].ClosestPoint(transform.position), Quaternion.identity);

            player.GetComponent<PlayerHealth>().GetDamage(damage / shield[0].GetComponent<HandheldItem>().GetShieldEffectiveness());
        }
        else if (Physics.CheckSphere(damageSphere.position, damageRange, playerLayer))
        {
            player.GetComponent<PlayerHealth>().GetDamage(damage);
            Instantiate(bloodSplat, player.gameObject.GetComponent<CapsuleCollider>().ClosestPoint(transform.position), Quaternion.identity);
        }
    }

    public void BecomeAggro()
    {
        if (state != State.Attack && state != State.Chase && state != State.Flee)
        {
            hostile = true;
            state = State.Chase;
            hostileTimer = hostileTime;

            AlertNearbyEnemiesWhenAggro();
        }
    }
    #endregion

    #region Getters
    public bool GetIsAggro()
    {
        return hostile;
    }
    #endregion

    #region GUI
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, alertOtherEnemiesRadius);
        
    }
    #endregion
}
