using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject startRoom;
    private GameObject currentRoom;
    [SerializeField] private List<GameObject> rooms;

    private GameObject player;
    private GameObject camera;

    private void Start()
    {
        currentRoom = startRoom;
        player = GameObject.FindGameObjectWithTag("Player");
        camera = GameObject.FindGameObjectWithTag("CameraPoint");
    }

    public void SpawnNewRoom(int enteredDoorSide)
    {
        var spawnPos = currentRoom.transform.position;
        var spawnSide = 0;
        switch (enteredDoorSide)
        {
            case 0:
                spawnPos += new Vector3(-38, 0, 0);
                spawnSide = 2;
                break;
            case 1:
                spawnPos += new Vector3(0, 22, 0);
                spawnSide = 3;
                break;
            case 2:
                spawnPos += new Vector3(38, 0, 0);
                spawnSide = 0;
                break;
            case 3:
                spawnPos += new Vector3(0, -22, 0);
                spawnSide = 1;
                break;
        }

        camera.transform.position = spawnPos;
        var newRoom = Instantiate(PickNewRoom(), spawnPos, Quaternion.identity);
        newRoom.GetComponent<Room>().doors[spawnSide].GetComponent<Door>().SpawnPlayer(player);
        currentRoom = newRoom;
    }

    private GameObject PickNewRoom()
    {
        return rooms[Random.Range(0, rooms.Count)];
    }
}
