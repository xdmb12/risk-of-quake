using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthSystem : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public Image healthBar;
    public TMP_Text healthText;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(0);
        }

        healthBar.fillAmount = health / maxHealth;
        healthText.text = $"{health}";

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Fireball"))
        {
            health -= other.gameObject.GetComponent<Fireball>().damage;
        }
    }
}
