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
    public GameObject cibe;

    public bool isReloading;

    private void Start()
    {
        weapon = weapons[currentWeapon];
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !isReloading)
        {
            if (weapon.bullets > 0)
            {
                if(weapon.shootingSpeed >= weapon.maxShootingSpeed)
                {
                    weapon.bullets--;
                    weapon.shootingSpeed = 0;

                    Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                    RaycastHit hit;
                    
                    newTracer = Instantiate(tracer, weapon.shootingPoint.position, Quaternion.identity, null);
                    // GameObject newCibe = Instantiate(cibe, weapon.shootingPoint);
                    // newCibe.transform.parent = null;
                    

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
}
