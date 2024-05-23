using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public int HP;


    private Animator Anim;
    private int layer;


    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public GameObject projectile2;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;
    private bool isDamaged;

    // Cache hash values
    private int MoveState;
    private int DamagedState;
    private int AttackState;
    private int AttackState2;
    private int DissolveState;


    // dissolve
    [SerializeField] private SkinnedMeshRenderer[] MeshR;
    private float Dissolve_value = 1;
    //private bool DissolveFlg = false;


    private const int Die = 0;
    private const int Move = 1;
    private const int Attack = 2;
    private const int Damaged = 3;
    public Dictionary<int, bool> status = new Dictionary<int, bool>
    {
        {Die, false },
        {Damaged, false },
        {Move, false },
        {Attack, false }
    };

    void OnEnabe()
    {
        player = GameManager.instance.player.GetComponent<Rigidbody>().transform;        
    }

    private void Awake()
    {
        //player = GameObject.Find("XR Origin (XR Rig)").transform;
        agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        string currentTag = gameObject.tag;

        if (currentTag == "Ghost")
        {
            layer = 0;
            Anim.SetLayerWeight(0, 1);
            MoveState = Animator.StringToHash("Ghost Layer.move");
            DamagedState = Animator.StringToHash("Ghost Layer.damaged");
            AttackState = Animator.StringToHash("Ghost Layer.attack_shift");
            DissolveState = Animator.StringToHash("Ghost Layer.dissolve");
        }
        else if (currentTag == "Mummy")
        {
            layer = 1;
            Anim.SetLayerWeight(1, 1);
            MoveState = Animator.StringToHash("Mummy Layer.Move");
            DamagedState = 0;
        }
        else if (currentTag == "Golem")
        {
            layer = 2;
            Anim.SetLayerWeight(2, 1);
            MoveState = Animator.StringToHash("Golem Layer.Walk");
            DamagedState = Animator.StringToHash("Golem Layer.GetHit");
            AttackState = Animator.StringToHash("Golem Layer.Attack01");
            AttackState2 = Animator.StringToHash("Golem Layer.Attack02");
            DissolveState = Animator.StringToHash("Golem Layer.Die");
        }
    }


    public bool setDamageState()
    {
        if (status[Damaged] == false)
            isDamaged = true;

        return isDamaged;
    }

    private void Update()
    {
        STATUS();
        if (status.ContainsValue(true))
        {
            int status_name = 0;
            foreach (var i in status)
                if (i.Value == true)
                {
                    status_name = i.Key;
                    break;
                }

            if (status_name == Die)
                Dissolve();
            else if (status_name == Damaged)
            {
                TakeDamage(1);
                isDamaged = false;
            }
            else if (status_name == Move)
                ChasePlayer();
            else if (status_name == Attack)
                AttackPlayer();


        }
    }

    private void STATUS()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // during die
        if (HP <= 0)
            status[Die] = true;
        //else if (!DissolveFlg)
        //    status[Die] = false;

        // during moving & attacking
        if ((playerInSightRange && playerInAttackRange))
        {
            status[Attack] = true;
            status[Move] = false;
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            status[Attack] = false;
            status[Move] = true;
        }
        else if (!playerInSightRange)
        {
            status[Attack] = false;
            status[Move] = false;
            //agent.SetDestination(transform.position);
        }

        // during damaging
        if (isDamaged)
            status[Damaged] = true;
        else if (!isDamaged)
            status[Damaged] = false;
        //else if (Anim.GetCurrentAnimatorStateInfo(layer).fullPathHash != DamagedState)
        //status[Damaged] = false;
    }

    private void Dissolve()
    {

        Dissolve_value -= Time.deltaTime;
        for (int i = 0; i < MeshR.Length; i++)
        {
            MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
        }
        if (Dissolve_value <= 0)
        {
            DestroyEnemy();
        }
        //Anim.CrossFade(DissolveState, 0.1f, 0, 0);
        //DissolveFlg = true;
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
        Anim.Play(MoveState);
        transform.LookAt(player);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code here
            if (layer == 2 && Physics.CheckSphere(transform.position, attackRange / 2, whatIsPlayer))
            {
                Anim.CrossFade(AttackState2, 0.15f, layer, 0.3f);
                Instantiate(projectile2);
            }
            else
            {
                Anim.CrossFade(AttackState, 0.15f, layer, 0.3f);
                InstantiateProjectile(projectile);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void InstantiateProjectile(GameObject projectile)
    {
        Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 4f, ForceMode.Impulse);
        rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        Anim.CrossFade(DamagedState, 0.1f, layer, 0);
        //rigidbody.AddForce(transform.forward * -2f, ForceMode.Impulse);
        HP -= damage;
        if (HP <= 0)
        {
            //Dissolve();
            status[Die] = true;
            //DissolveFlg = true;
        }
    }
    private void DestroyEnemy()
    {
        DestroyImmediate(gameObject, true);
        DestroyImmediate(projectile, true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}