using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    public Transform[] points;
    public LayerMask whatIsPlayer;
    private Vector3 target;
    [HideInInspector] public Transform player;

    public float attackCooldown;
    public int currentPoint = 0;
    private bool alreadyAttacked;
    

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdateForTarget());
    }

    private void FixedUpdate()
    {
        var playerInAttackRange = Physics.CheckSphere(transform.position, 10f, whatIsPlayer);
        
        if (player)
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
        else
        {
            Patroling();
        }
    }

    private void ChasingPlayer()
    {
        target = player.position;
    }

    private void Patroling()
    {
        target = points[currentPoint].position;

        if (Vector3.Distance(transform.position, points[currentPoint].position) < 0.5f)
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

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 directionToTarget = (other.transform.position + Vector3.up) - (transform.position + Vector3.up);
            RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up, directionToTarget);
            
            if(Physics.Raycast(ray, out hit, 20f))
            {
                Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    player = other.transform;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.up, target + Vector3.up);
    }
}
