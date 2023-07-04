using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Transform shootingPoint;
    public ParticleSystem particle;
    
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

    public enum TypeOfWeapon
    {
        pistol,
        melee
    }
    public TypeOfWeapon type;
    

    private void Update()
    {
        if(type == TypeOfWeapon.pistol)
        {
            bulletsText.text = $"{bullets}/{bulletsMax}";
            bulletsAllText.text = $"{bulletsAll}";

            reloadingSlide.fillAmount = reloading / maxReloading;
        }
    }
}
