using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Weapon[] weapons;
    private Weapon weapon;
    public int currentWeapon;
    public float maxDistance;


    [SerializeField] private Transform lookAt;
    public Camera mainCamera;

    public LayerMask targetLayer;
    public GameObject tracer;
    private GameObject newTracer;
    public Animator animator;

    public bool isReloading;

    private void Start()
    {
        weapon = weapons[currentWeapon];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponRefresh(0);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponRefresh(1);
        }
        
        if(Input.GetMouseButton(0))
        {
            if (!isReloading && weapon.type == Weapon.TypeOfWeapon.pistol)
            {
                if (weapon.bullets > 0)
                {
                    if (weapon.shootingSpeed >= weapon.maxShootingSpeed)
                    {
                        weapon.bullets--;
                        weapon.shootingSpeed = 0;

                        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                        RaycastHit hit;

                        newTracer = Instantiate(tracer, weapon.shootingPoint.position, Quaternion.identity, null);
                        newTracer.GetComponent<TracerScript>().target = mainCamera.transform.GetChild(0).position;


                        if (Physics.Raycast(ray, out hit, maxDistance, targetLayer))
                        {

                            Debug.Log(hit.collider.gameObject.name);
                            if (hit.collider.GetComponent<EnemyDamage>())
                            {
                                hit.collider.GetComponent<EnemyDamage>().Damage(weapon.damage);
                            }
                        }

                        if (weapon.bullets == 0)
                            ReloadingStart();
                    }
                }
                else
                {
                    ReloadingStart();
                }
            }

            if (weapon.type == Weapon.TypeOfWeapon.melee)
            {
                animator.CrossFade("Sword attack", 0.1f);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            ReloadingStart();
        }
    }

    private void FixedUpdate()
    {
        if (weapon.shootingSpeed < weapon.maxShootingSpeed)
        {
            weapon.shootingSpeed += Time.deltaTime * 10f;
        }

        if (weapon.reloading < weapon.maxReloading)
        {
            weapon.reloading++;
        }
        else
        {
            if(isReloading) 
                ReloadingFinish();
        }
    }

    void ReloadingStart()
    {
        weapon.reloading = 0;
        isReloading = true;
    }

    void ReloadingFinish()
    {
        if (weapon.bullets >= 0 && weapon.bullets < weapon.bulletsMax)
        {
            float remainingBullets = weapon.bulletsMax - weapon.bullets;
        
            if (weapon.bulletsAll >= remainingBullets)
            {
                weapon.bulletsAll -= remainingBullets;
                weapon.bullets = weapon.bulletsMax;
            }
            else
            {
                weapon.bullets += weapon.bulletsAll;
                weapon.bulletsAll = 0;
            }
        }
        else if (weapon.bulletsAll <= weapon.bulletsMax)
        {
            weapon.bullets = weapon.bulletsAll;
            weapon.bulletsAll = 0;
        }

        isReloading = false;
    }

    void WeaponRefresh(int newWeapon)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }
        
        weapons[newWeapon].gameObject.SetActive(true);

        weapon = weapons[newWeapon];
    }
}
