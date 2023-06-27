using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int currentZone;

    private List<SpawnZone> spawnZones = new List<SpawnZone>();

    public GameObject enemyPrefab;

    public float timer;
    public float timerMax;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            spawnZones.Add(transform.GetChild(i).GetComponent<SpawnZone>());
        }
    }

    private void FixedUpdate()
    {
        if (timer >= timerMax) 
        {
            timer = 0;
            Instantiate(enemyPrefab, spawnZones[currentZone].FindSpawnPosition(), Quaternion.identity, null);
        }
        else 
        {
            timer++;
        }
    }
}
