using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Transform shootingPoint;
    
    [Header("Bullets")]
    public float bullets;
    public float bulletsMax;
    public float bulletsAll;
    
    [Header("Shooting")]
    public float shootingSpeed;
    public float maxShootingSpeed;
    public float damage;

    [Header("Reloading")] 
    public float reloading;
    public float maxReloading;
    
    [Header("Canvas")]
    public TMP_Text bulletsText;
    public TMP_Text bulletsAllText;
    public Image reloadingSlide;
    

    private void Update()
    {
        bulletsText.text = $"{bullets}/{bulletsMax}";
        bulletsAllText.text = $"{bulletsAll}";

        reloadingSlide.fillAmount = reloading / maxReloading;
    }
}
