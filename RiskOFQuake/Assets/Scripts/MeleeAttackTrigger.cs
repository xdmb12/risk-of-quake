using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTrigger : MonoBehaviour
{
    private MeleeWeapon meleeWeapon;

    private EnemyDamage damaged;

    private void Start()
    {
        meleeWeapon = GetComponent<MeleeWeapon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyDamage>())
        {
            if (!damaged) 
            {
                other.gameObject.GetComponent<EnemyDamage>().Damage(meleeWeapon.damage);
                StartCoroutine(DeleteDamaged());
            }

            damaged = other.gameObject.GetComponent<EnemyDamage>();
        }
    }

    IEnumerator DeleteDamaged() 
    {
        yield return new WaitForSeconds(0.7f);
        damaged = null;
    }
}
