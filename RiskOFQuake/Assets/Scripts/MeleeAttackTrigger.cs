using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyDamage>())
        {
            other.gameObject.GetComponent<EnemyDamage>().Damage(20);
        }
    }
}
