using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    public Transform[] points;
    [HideInInspector] public Transform player;
    private Vector3 target;
    public int currentPoint = 0;
    

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdateForTarget());
    }

    private void FixedUpdate()
    {
        if (player)
        {
            target = player.position;
        }
        else
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
    }

    IEnumerator UpdateForTarget()
    {
        yield return new WaitForSeconds(0.25f);
        _navMeshAgent.SetDestination(target);
        StartCoroutine(UpdateForTarget());
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
