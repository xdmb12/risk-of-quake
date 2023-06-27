using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    private Transform leftTop;
    private Transform rightDown;

    private void Start()
    {
        leftTop = transform.GetChild(0);
        rightDown = transform.GetChild(1);
    }

    public Vector3 FindSpawnPosition() 
    {
        float x = Random.Range(leftTop.position.x, rightDown.position.x);
        float z = Random.Range(leftTop.position.z, rightDown.position.z);

        Vector3 spawnPosition = new Vector3(x, leftTop.position.y, z);

        return spawnPosition;
    }
}
