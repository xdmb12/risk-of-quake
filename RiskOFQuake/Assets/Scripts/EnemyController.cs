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
    [HideInInspector] public PlayerMovement pm;
    private Spawner spawner;

    private void Awake()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        pm = gameManager.player.GetComponent<PlayerMovement>();
        spawner = gameManager.spawner;
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
            pm.runningSpeed = pm.maxRunningSpeed;
            Destroy(gameObject);

            spawner.CheckEnemys();
        }
    }
}
