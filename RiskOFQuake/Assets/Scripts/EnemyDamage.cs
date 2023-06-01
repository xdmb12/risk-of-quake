using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private EnemyController _enemyController;
    public float coef;

    private void Start()
    {
        _enemyController = transform.parent.GetComponent<EnemyController>();
    }

    public void Damage(float originalDamage)
    {
        _enemyController.HealthChange(-originalDamage * coef);
    }
}
