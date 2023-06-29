using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Transform enemyContainer;
    public Spawner spawner;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
