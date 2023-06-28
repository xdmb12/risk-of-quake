using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarCanvasController : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        player = playerObject;
    }

    private void Update()
    {
        transform.LookAt(player.transform);
    }
}
