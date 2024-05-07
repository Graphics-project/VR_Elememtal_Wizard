using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;

    private Animator Anim;
    private CharacterController Ctrl;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange = true;  // This is set to disable patrolling
    public bool playerInAttackRange;

    // Cache hash values
    private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
    private static readonly int SurprisedState = Animator.StringToHash("Base Layer.surprised");
    private static readonly int AttackState = Animator.StringToHash("Base Layer.attack_shift");
    private static readonly int DissolveState = Animator.StringToHash("Base Layer.dissolve");
    private static readonly int AttackTag = Animator.StringToHash("Attack");
    // dissolve
    [SerializeField] private SkinnedMeshRenderer[] MeshR;
    private float Dissolve_value = 1;
    private bool DissolveFlg = false;
    private const int maxHP = 3;
    private int HP = maxHP;


    private const int Dissolve = 1;
    private const int Attack = 2;
    private const int Surprised = 3;
    private Dictionary<int, bool> PlayerStatus = new Dictionary<int, bool>
    {
        {Dissolve, false },
        {Attack, false },
        {Surprised, false },
    };

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
        Anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        STATUS();
        // this character status
        if (PlayerStatus.ContainsValue(true))
        {
            int status_name = 0;
            foreach (var i in PlayerStatus)
            {
                if (i.Value == true)
                {
                    status_name = i.Key;
                    break;
                }
            }
            if (status_name == Dissolve)
            {
                PlayerDissolve();
            }
            else if (status_name == Surprised)
            {
                // nothing method
            }
        }
        // Dissolve
        if (HP <= 0 && !DissolveFlg)
        {
            Anim.CrossFade(DissolveState, 0.1f, 0, 0);
            DissolveFlg = true;
        }

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // if (!playerInSightRange && !playerInAttackRange) Patrolling(); // This is set to disable patrolling
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void STATUS()
    {
        // during dissolve
        if (DissolveFlg && HP <= 0)
        {
            PlayerStatus[Dissolve] = true;
        }
        else if (!DissolveFlg)
        {
            PlayerStatus[Dissolve] = false;
        }
        // during attacking
        if (Anim.GetCurrentAnimatorStateInfo(0).tagHash == AttackTag)
        {
            PlayerStatus[Attack] = true;
        }
        else if (Anim.GetCurrentAnimatorStateInfo(0).tagHash != AttackTag)
        {
            PlayerStatus[Attack] = false;
        }
        // during damaging
        if (Anim.GetCurrentAnimatorStateInfo(0).fullPathHash == SurprisedState)
        {
            PlayerStatus[Surprised] = true;
        }
        else if (Anim.GetCurrentAnimatorStateInfo(0).fullPathHash != SurprisedState)
        {
            PlayerStatus[Surprised] = false;
        }
    }
    // dissolve shading
    private void PlayerDissolve()
    {
        Dissolve_value -= Time.deltaTime;
        for (int i = 0; i < MeshR.Length; i++)
        {
            MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
        }
        if (Dissolve_value <= 0)
        {
            Ctrl.enabled = false;
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        transform.LookAt(player);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            Anim.CrossFade(AttackState, 0.15f, 0, 0);
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
            rb.AddForce(transform.up * 4f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        Anim.CrossFade(SurprisedState, 0.1f, 0, 0);
        health -= damage;
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
        Destroy(projectile);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}