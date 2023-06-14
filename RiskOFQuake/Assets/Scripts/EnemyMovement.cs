using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] points;
    public LayerMask whatIsPlayer;
    public Transform player;
    private NavMeshAgent _navMeshAgent;
    private Vector3 target;

    public float attackCooldown;
    public int currentPoint = 0;
    public float visionDistance;
    private bool alreadyAttacked;

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
        playerInAttackRange = Physics.CheckSphere(transform.position + Vector3.up, 10f, whatIsPlayer);
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
        state = EnemyState.Chasing;
    }

    private void Patrolling()
    {
        target = points[currentPoint].position;
        state = EnemyState.Patrolling;

        if (Vector3.Distance(transform.position, points[currentPoint].position) < 1f)
        {
            currentPoint++;

            if (currentPoint == points.Length)
            {
                currentPoint = 0;
            }
        }
    }
    void AttackPlayer()
    {
        target = transform.position;
        state = EnemyState.Attacking;
        
        transform.LookAt(player.position);
        if(!alreadyAttacked)
        {
            //here must be attack
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
