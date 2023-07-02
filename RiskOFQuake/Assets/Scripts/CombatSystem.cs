 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
 using Random = UnityEngine.Random;

 public class CombatSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    
    [Header("Shooting Weapon")]
    [SerializeField] private float maxDistance;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private GameObject tracer;
    [SerializeField] private float weaponSpreadX;
    [SerializeField] private float weaponSpreadY;
    private Weapon shootingWeapon;
    private GameObject newTracer;
    private bool isReloading;

    [Header("Melee Weapon")] 
    public MeleeWeapon meleeWeapon;
    
    

    [Header("Anims")]
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Rig handsRigLayer;
    
    [Header("Input")]
    [SerializeField] private KeyCode pistolKey = KeyCode.Alpha1;
    [SerializeField] private KeyCode swordKey = KeyCode.Alpha2;
    [SerializeField] private KeyCode handsKey = KeyCode.Alpha3;
    
    public TypeOfWeapon type;
    public enum TypeOfWeapon
    {
        Pistol,
        Melee,
        Hands
    }

    private void Start()
    {
        type = TypeOfWeapon.Hands;
        shootingWeapon = weapons[0].gameObject.GetComponent<Weapon>();
        meleeWeapon.maxMeleeAttackCooldown = meleeWeapon.meleeAttackCooldown;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(type == TypeOfWeapon.Melee)
            {
                MeleeWeapon();
            }
            
            if(type == TypeOfWeapon.Pistol)
            {
                Debug.Log("Pew");
                ShootingWeapon();
            }

            if (type == TypeOfWeapon.Hands)
            {
                Debug.Log("Invalid");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && type == TypeOfWeapon.Pistol)
        {
            ReloadingStartForShootingWeapon();
        }
        
        WeaponChanger();
    }

    private void FixedUpdate()
    {
        if(type == TypeOfWeapon.Pistol)
        {
            RateOfFireForShootingWeapon();
        }
        
        if(type == TypeOfWeapon.Melee)
        {
            MeleeWeaponAttackRate();
        }
        
    }

    void WeaponChanger()
    {
        if(Input.GetKeyDown(pistolKey))
        {
            type = TypeOfWeapon.Pistol;
            handsRigLayer.weight = 1;
            weapons[2].SetActive(false);
            weapons[0].SetActive(true);
            //weapons[1].SetActive(true);
        }
        
        if (Input.GetKeyDown(swordKey))
        {
            type = TypeOfWeapon.Melee;
            handsRigLayer.weight = 0;
            weapons[0].SetActive(false);
            //weapons[1].SetActive(false);
            weapons[2].SetActive(true);
        }

        if (Input.GetKeyDown(handsKey))
        {
            type = TypeOfWeapon.Hands;
            handsRigLayer.weight = 0;
            weapons[0].SetActive(false);
            //weapons[1].SetActive(false);
            weapons[2].SetActive(false);
        }
        
    }

    void MeleeWeapon()
    {
        if(meleeWeapon.meleeAttackCooldown >= meleeWeapon.maxMeleeAttackCooldown)
        {
            meleeWeapon.meleeAttackCooldown = 0;
            playerAnim.SetTrigger("MeleeAttack");
        }
    }

    void MeleeWeaponAttackRate()
    {
        if (meleeWeapon.meleeAttackCooldown < meleeWeapon.maxMeleeAttackCooldown)
        {
            meleeWeapon.meleeAttackCooldown += Time.deltaTime * 10f;
        }
    }

    void ShootingWeapon()
    {
        if (!isReloading)
        {
            if (shootingWeapon.bullets > 0)
            {
                if (shootingWeapon.shootingSpeed >= shootingWeapon.maxShootingSpeed)
                {
                    shootingWeapon.bullets--;
                    shootingWeapon.shootingSpeed = 0;

                    Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                    RaycastHit hit;

                    newTracer = Instantiate(tracer, shootingWeapon.shootingPoint.position, Quaternion.identity, null);
                    newTracer.GetComponent<TracerScript>().target = mainCamera.transform.GetChild(0).position;


                    if (Physics.Raycast(ray, out hit, maxDistance, targetLayer))
                    {

                        Debug.Log(hit.collider.gameObject.name);
                        if (hit.collider.GetComponent<EnemyDamage>())
                        {
                            hit.collider.GetComponent<EnemyDamage>().Damage(shootingWeapon.damage);
                        }
                    }

                    if (shootingWeapon.bullets == 0)
                        ReloadingStartForShootingWeapon();
                }
            }
            else
            {
                ReloadingStartForShootingWeapon();
            }
        }
    }

    Vector3 WeaponShootingSpread()
    {
        float x = Random.Range(-weaponSpreadX, weaponSpreadX);
        float y = Random.Range(-weaponSpreadY, weaponSpreadY);

        Vector3 weaponSpread = new Vector3(x, y, transform.position.z);

        return weaponSpread;
    }

    void RateOfFireForShootingWeapon()
    {
        if (shootingWeapon.shootingSpeed < shootingWeapon.maxShootingSpeed)
        {
            shootingWeapon.shootingSpeed += Time.deltaTime * 10f;
        }

        if (shootingWeapon.reloading < shootingWeapon.maxReloading)
        {
            shootingWeapon.reloading++;
        }
        else
        {
            if (isReloading)
                ReloadingFinishForShootingWeapon();
        }
    }
    
    void ReloadingStartForShootingWeapon()
    {
        shootingWeapon.reloading = 0;
        isReloading = true;
    }
    
    void ReloadingFinishForShootingWeapon()
    {
        if (shootingWeapon.bullets >= 0 && shootingWeapon.bullets < shootingWeapon.bulletsMax)
        {
            float remainingBullets = shootingWeapon.bulletsMax - shootingWeapon.bullets;
        
            if (shootingWeapon.bulletsAll >= remainingBullets)
            {
                shootingWeapon.bulletsAll -= remainingBullets;
                shootingWeapon.bullets = shootingWeapon.bulletsMax;
            }
            else
            {
                shootingWeapon.bullets += shootingWeapon.bulletsAll;
                shootingWeapon.bulletsAll = 0;
            }
        }
        else if (shootingWeapon.bulletsAll <= shootingWeapon.bulletsMax)
        {
            shootingWeapon.bullets = shootingWeapon.bulletsAll;
            shootingWeapon.bulletsAll = 0;
        }

        isReloading = false;
    }
}
