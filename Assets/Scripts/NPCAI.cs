using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPCAI : MonoBehaviour
{
    /*[SerializeField] private GameObject destinationPoint;

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(destinationPoint.transform.position);
    }*/

    public NavMeshAgent _agent;
    [SerializeField] Transform _player;
    public LayerMask ground, player;

    public Vector3 destinationPoint;
    private bool destinationPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public GameObject sphere;

    public float sightRange, attackRange;
    public bool playerInsightRange, playerAttackRange;


    // animation IDs
    private int _animIDisFar;
    private int _animIDisAttacking;
    private Animator _animator;

    private bool beingHandled = false;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }   

    private void Update()
    {
        playerInsightRange = Physics.CheckSphere(transform.position, sightRange,player);
        playerAttackRange = Physics.CheckSphere(transform.position, attackRange,player);

        //Patrol / Chase / Attack

        if (!playerInsightRange && !playerAttackRange) Patroling();
        if (playerInsightRange && !playerAttackRange) ChasePlayer();
        if (playerInsightRange && playerAttackRange) AttackPlayer();

    }

    void Patroling()
    {
        if(!destinationPointSet)
        {
            SearchWalkPoint();
        }

        if(destinationPointSet)
        {
            _agent.SetDestination(destinationPoint);
        }

        Vector3 distanceToDestinationPoint = transform.position - destinationPoint;
        if(distanceToDestinationPoint.magnitude < 1.0f)
        {
            destinationPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        _animator.SetBool("isFar", true);
        _animator.SetBool("isAttacking", false);
        float randomX = UnityEngine.Random.Range(-walkPointRange,walkPointRange);
        float randomZ = UnityEngine.Random.Range(-walkPointRange,walkPointRange);

        destinationPoint = new Vector3(transform.position.x + randomX, transform.position.y,transform.position.z + randomZ);

        if(Physics.Raycast(destinationPoint, -transform.up, 2.0f, ground))
        {
            destinationPointSet = true;
        }
    }

    void ChasePlayer()
    {
        _animator.SetBool("isFar", true);
        _animator.SetBool("isAttacking", false);
        _agent.SetDestination(_player.position); 
    }

    void AttackPlayer()
    {
        _animator.SetBool("isFar", false);
        _agent.SetDestination(transform.position);

        transform.LookAt(_player);
        if (!alreadyAttacked)
        {
            if(!beingHandled)
            {
                StartCoroutine( HandleIt() );
            }    
        }
    }

    IEnumerator ChangeTag(GameObject gameObject)
    {
        yield return new WaitForSeconds(2);
        gameObject.tag = "Respawn";
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private IEnumerator HandleIt()
    {
        beingHandled = true;
        _animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds( 0.82f ); //0.49 seconds
        var pos = new Vector3(transform.position.x,transform.position.y + 1.2f,
        transform.position.z);

        Rigidbody rb = Instantiate(sphere, pos ,Quaternion.identity).GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 25f, ForceMode.Impulse);
        rb.AddForce(transform.up * 3f, ForceMode.Impulse);

        StartCoroutine(ChangeTag(rb.gameObject));
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        beingHandled = false;
    }

}
