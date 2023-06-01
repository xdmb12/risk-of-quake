using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Weapon[] weapons;
    public int currentWeapon;
    public float maxDistance;


    [SerializeField] private Transform lookAt;
    public Camera mainCamera;

    public LayerMask targetLayer;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, targetLayer))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.GetComponent<EnemyDamage>())
                {
                    hit.collider.GetComponent<EnemyDamage>().Damage(weapons[currentWeapon].damage);
                }
            }
            else
            {
                
            }
        }
    }
}
