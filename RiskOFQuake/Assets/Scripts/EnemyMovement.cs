using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] points;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;
    public Transform player;
    public GameObject fireball;
    public Transform shootingPoint;
    private NavMeshAgent _navMeshAgent;
    private Vector3 target;
    private Vector3 randomPointTarget;
    private GameObject newFireball;

    public float attackCooldown;
    public int currentPoint = 0;
    public int walkPointRange;
    public float visionDistance;
    public float attackDistance;
    private bool alreadyAttacked;
    private bool walkPointSet;

    private bool playerInAttackRange;
    private bool playerInVisionRange;
    
    private EnemyState state;
    
    private enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking
    }
    

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdateForTarget());
    }

    private void FixedUpdate()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position + Vector3.up, attackDistance, whatIsPlayer);
        playerInVisionRange = Physics.CheckSphere(transform.position + Vector3.up, visionDistance, whatIsPlayer);
        
        if (playerInVisionRange)
        {
            Vector3 directionToTarget = (player.transform.position + Vector3.up) - (transform.position + Vector3.up);
            RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up, directionToTarget);
            
            if(Physics.Raycast(ray, out hit, visionDistance))
            {
                Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    if (playerInAttackRange)
                    {
                        AttackPlayer();
                    }
                    else
                    {
                        ChasingPlayer();
                    }
                }
            }
            
        }
        else
        {
            Patrolling();
        }
        
        Debug.Log($"State: {state}");
    }

    private void ChasingPlayer()
    {
        target = player.position;
        walkPointSet = true;
        state = EnemyState.Chasing;
    }

    
    private void Patrolling()
    {
        state = EnemyState.Patrolling;

        if (points.Length > 1)
        {
            target = points[currentPoint].position;
            
            Vector3 distanceToWalkPoint = transform.position - target;

            if (distanceToWalkPoint.magnitude < 1f)
            {
                currentPoint++;

                if (currentPoint == points.Length)
                {
                    currentPoint = 0;
                }
            }
        }
        else
        {
            if(!walkPointSet) 
                SearchRandomPoint();

            if (walkPointSet)
                target = randomPointTarget;
            

            Vector3 distanceToWalkPoint = transform.position - target;

            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;

        }
    }

    void SearchRandomPoint()
    {
        Random rd = new Random();
        int randomZ = rd.Next(-walkPointRange, walkPointRange);
        int randomX = rd.Next(-walkPointRange, walkPointRange);
        Debug.Log($"newPoint{randomZ} {randomZ}");

        randomPointTarget = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(randomPointTarget, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    void AttackPlayer()
    {
        target = transform.position;
        
        state = EnemyState.Attacking;
        
        transform.LookAt(player.position); 

        if(!alreadyAttacked)
        {
            newFireball = Instantiate(fireball, shootingPoint.position, Quaternion.identity, null);
            newFireball.transform.LookAt(player.position + Vector3.up);
            
            Debug.Log("Attack");
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private IEnumerator UpdateForTarget()
    {
        yield return new WaitForSeconds(0.15f);
        _navMeshAgent.SetDestination(target);
        StartCoroutine(UpdateForTarget());
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.up, target + Vector3.up);
        Gizmos.DrawWireSphere(transform.position + Vector3.up, visionDistance);
    }
}
