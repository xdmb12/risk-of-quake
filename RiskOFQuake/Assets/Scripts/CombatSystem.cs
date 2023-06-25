using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CombatSystem : MonoBehaviour
{
    public Animator playerAnim;
    public GameObject[] weapons;
    public Rig handsRigLayer;

    public KeyCode pistolKey = KeyCode.Alpha1;
    public KeyCode swordKey = KeyCode.Alpha2;
    public KeyCode handsKey = KeyCode.Alpha3;
    
    public TypeOfWeapon type;
    public enum TypeOfWeapon
    {
        pistol,
        melee,
        hands
    }

    private void Start()
    {
        type = TypeOfWeapon.hands;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(type == TypeOfWeapon.melee)
            {
                playerAnim.SetTrigger("MeleeAttack");
            }
            
            if(type == TypeOfWeapon.pistol)
            {
                Debug.Log("Pew");
            }

            if (type == TypeOfWeapon.hands)
            {
                Debug.Log("Invalid");
            }
        }
        
        WeaponChanger();
    }

    void WeaponChanger()
    {
        if(Input.GetKeyDown(pistolKey))
        {
            type = TypeOfWeapon.pistol;
            handsRigLayer.weight = 1;
            weapons[1].SetActive(false);
            weapons[0].SetActive(true);
        }
        
        if (Input.GetKeyDown(swordKey))
        {
            type = TypeOfWeapon.melee;
            handsRigLayer.weight = 0;
            weapons[0].SetActive(false);
            weapons[1].SetActive(true);
        }

        if (Input.GetKeyDown(handsKey))
        {
            type = TypeOfWeapon.hands;
            handsRigLayer.weight = 0;
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
        }
        
    }
}
