using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Slider slider;
    private PlayerMovement pm;

    private void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        pm = playerObject.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        slider.value = health / maxHealth;
    }

    public void HealthChange(float count)
    {
        health += count;

        health = Mathf.Clamp(health, 0, maxHealth);

        if (health == 0)
        {
            Destroy(gameObject);
            pm.runningSpeed = pm.maxRunningSpeed;
        }
            
    }
}
