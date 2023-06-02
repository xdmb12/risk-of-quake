using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform shootingPoint;
    public float damage;
    
    [Header("Bullets")]
    public float bullets;
    public float bulletsMax;
    public float bulletsAll;
    
    [Header("Canvas")]
    public TMP_Text bulletsText;
    public TMP_Text bulletsAllText;

    private void Update()
    {
        bulletsText.text = $"{bullets}/{bulletsMax}";
        bulletsAllText.text = $"{bulletsAll}";
    }
}
