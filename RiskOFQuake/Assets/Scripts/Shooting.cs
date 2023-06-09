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

    public float shootingSpeed;
    public float maxShootingSpeed;


    [SerializeField] private Transform lookAt;
    public Camera mainCamera;

    public LayerMask targetLayer;
    public GameObject tracer;

    private void Start()
    {
        weapon = weapons[currentWeapon];
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (weapon.bullets > 0)
            {
                if(shootingSpeed >= maxShootingSpeed)
                {
                    weapon.bullets--;
                    shootingSpeed = 0;

                    Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                    RaycastHit hit;
                    
                    GameObject newTracer = Instantiate(tracer, weapon.transform.position, Quaternion.identity, null);
                    newTracer.GetComponent<TracerScript>().target =  mainCamera.transform.GetChild(0).position;
                    

                    if (Physics.Raycast(ray, out hit, maxDistance, targetLayer))
                    {

                        Debug.Log(hit.collider.gameObject.name);
                        if (hit.collider.GetComponent<EnemyDamage>())
                        {
                            hit.collider.GetComponent<EnemyDamage>().Damage(weapon.damage);
                        }
                    }

                    if (weapon.bullets == 0)
                        Reloading();
                }
            }
            else
            {
                Reloading();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reloading();
        }
    }

    private void FixedUpdate()
    {
        if (shootingSpeed < maxShootingSpeed)
        {
            shootingSpeed += Time.deltaTime * 10f;
        }
    }

    void Reloading()
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
    }
}
