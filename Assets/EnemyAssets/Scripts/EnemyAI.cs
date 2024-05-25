using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // public parameters
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public int HP;
    public Vector3 offset;

    private Animator Anim;
    private int layer;
    private float originalAgentSpeed;
    private float originalAnimatorSpeed;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public GameObject projectile2;

    //State parameters
    public float sightRange, attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    // Animation hash values
    private int MoveState;
    private int DamagedState;
    private int AttackState;
    private int AttackState2;
    private int DissolveState;

    // dissolve
    [SerializeField] private SkinnedMeshRenderer[] MeshR;
    private float Dissolve_value = 1;

    private const int Die = 0;
    private const int Move = 1;
    private const int Attack = 2;
    public Dictionary<int, bool> status = new Dictionary<int, bool>
    {
        {Die, false },
        {Move, false },
        {Attack, false }
    };


    void OnEnable()
    {
        if (player == null)
            player = GameManager.instance.player.GetComponent<Transform>();
    }

    private void Awake()
    {
        //if (player == null)
        //    player = GameManager.instance.player.GetComponent<Transform>();
        //player = GameObject.Find("XR Origin (XR Rig)").transform;
        agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        string currentTag = gameObject.tag;

        if (currentTag == "Ghost")
        {
            offset= new Vector3(0, -1.5f, 0);
            MoveState = Animator.StringToHash("Ghost Layer.move");
            DamagedState = Animator.StringToHash("Ghost Layer.damaged");
            AttackState = Animator.StringToHash("Ghost Layer.attack_shift");
            DissolveState = Animator.StringToHash("Ghost Layer.dissolve");
        }
        else if (currentTag == "Mummy")
        {
            offset = new Vector3(0, -1.5f, 0);
            MoveState = Animator.StringToHash("Mummy Layer.Move");
            DamagedState = 0;
        }
        else if (currentTag == "Golem")
        {
            MoveState = Animator.StringToHash("Golem Layer.Walk");
            DamagedState = Animator.StringToHash("Golem Layer.GetHit");
            AttackState = Animator.StringToHash("Golem Layer.Attack01");
            AttackState2 = Animator.StringToHash("Golem Layer.Attack02");
            DissolveState = Animator.StringToHash("Golem Layer.Die");
        }
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
            else if (status_name == Move)
                ChasePlayer();
            else if (status_name == Attack)
                AttackPlayer();


        }
    }

    private void STATUS()
    {
        //Check for sight and attack range
        if (gameObject.tag == "Golem") playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        else playerInSightRange = true;
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // during die
        if (HP <= 0)
            status[Die] = true;

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
        }

        // during damaging
        //if (isDamaged)
        //    status[Damaged] = true;
        //else if (!isDamaged)
        //    status[Damaged] = false;
    }

    private void Dissolve()
    {
        Dissolve_value -= Time.deltaTime;
        for (int i = 0; i < MeshR.Length; i++)
        {
            MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
        }
        if (Dissolve_value <= 0)
            DestroyEnemy();
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

        Vector3 lookAtPosition = player.position + offset;
        transform.LookAt(lookAtPosition);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        Vector3 lookAtPosition = player.position + offset;
        transform.LookAt(lookAtPosition);

        if (!alreadyAttacked)
        {
            if (projectile2 != null && Physics.CheckSphere(transform.position, attackRange / 2, whatIsPlayer))
            {
                Anim.CrossFade(AttackState2, 0.15f, 0, 0.3f);
                Instantiate(projectile2);
            }
            else
            {
                Anim.CrossFade(AttackState, 0.15f, 0, 0.3f);
                InstantiateProjectile(projectile);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void InstantiateProjectile(GameObject projectile)
    {
        
        Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
        rb.AddForce(transform.up * 6f, ForceMode.Impulse);
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {   
        Anim.CrossFade(DamagedState, 0.1f, 0, 0.2f);
        //rigidbody.AddForce(transform.forward * -2f, ForceMode.Impulse);
        HP -= damage;
        if (HP <= 0)
            status[Die] = true;
    }
    public void SlowDown()
    {
        originalAgentSpeed = agent.speed;
        originalAnimatorSpeed = Anim.speed;

        agent.speed = agent.speed / 2f > 0.5f ? agent.speed / 2f : 0.5f;
        Anim.speed = originalAnimatorSpeed / 2f;

        Invoke(nameof(ResetSpeedAndAnimation), 2f);
    }
    public void Stunned()
    {
        originalAgentSpeed = agent.speed;
        originalAnimatorSpeed = Anim.speed;

        alreadyAttacked = true;
        agent.speed = 0;
        Anim.speed = 0;

        Invoke(nameof(ResetSpeedAnimAttack), 2f);
    }
    private void ResetSpeedAndAnimation()
    {
        agent.speed = originalAgentSpeed;
        Anim.speed = originalAnimatorSpeed;
    }
    private void ResetSpeedAnimAttack()
    {
        ResetSpeedAndAnimation();
        alreadyAttacked = false;
    }


    public void DestroyEnemy()
    {
        //DestroyImmediate(gameObject, true);
        //gameObject.SetActive(false);
        Destroy(gameObject);
        GameManager.instance.decCurrentEnemyNum();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}