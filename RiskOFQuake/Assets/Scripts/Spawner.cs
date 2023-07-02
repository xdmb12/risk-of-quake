using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int currentZone;

    private List<SpawnZone> spawnZones = new List<SpawnZone>();

    public int currentWave;

    public SpawnWave[] waves;

    private Transform enemyContainer;

    private void Start()
    {
        enemyContainer = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().enemyContainer;

        for (int i = 0; i < transform.childCount; i++) 
        {
            spawnZones.Add(transform.GetChild(i).GetComponent<SpawnZone>());
        }

        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < waves[currentWave].prefabs.Length; i++) 
        {
            if(waves[currentWave].spawnPoint.Length != 0)
            {
                Instantiate(waves[currentWave].prefabs[i], waves[currentWave].spawnPoint[i], Quaternion.identity, enemyContainer);
            }
            else
            {
                Instantiate(waves[currentWave].prefabs[i], spawnZones[currentZone].FindSpawnPosition(),
                    Quaternion.identity, enemyContainer);
            }
        }

        //if (timer >= timerMax) 
        //{
        //    timer = 0;
        //    Instantiate(enemyPrefab, spawnZones[currentZone].FindSpawnPosition(), Quaternion.identity, null);
        //}
        //else 
        //{
        //    timer++;
        //}
    }

    public void NextWave() 
    {
        if (currentWave + 1 == waves.Length)
            return;

        currentZone++;

        if (currentZone + 1 == spawnZones.Count) 
        {
            currentZone = 0;
        }

        currentWave++;

        Spawn();
    }

    public void CheckEnemys()
    {
        if (enemyContainer.childCount == 1) 
        {
            NextWave();
        }
    }
}
