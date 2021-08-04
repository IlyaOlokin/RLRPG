using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Door : MonoBehaviour
{
    [NonSerialized] public bool isActive = true;
    private RoomManager rm;
    [SerializeField] private Transform spawnPos;
    
    // 0 - Left
    // 1 - Up
    // 2 - Right
    // 3 - Down
    [SerializeField] private int side;
    
    private void Start()
    {
        rm = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.gameObject.CompareTag("Player"))
        {
            rm.SpawnNewRoom(side);
        }
    }

    public void SpawnPlayer(GameObject player)
    {
        isActive = false;
        player.transform.position = transform.position + transform.up;
    }
}
