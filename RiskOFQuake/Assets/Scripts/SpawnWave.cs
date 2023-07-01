using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnWave", menuName = "ScriptableObjects/SpawnWaveData", order = 1)]
public class SpawnWave : ScriptableObject
{
    public GameObject[] prefabs;
    public Vector3[] spawnPoint;
}
