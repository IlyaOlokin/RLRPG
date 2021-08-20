using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
    // For dungeon generation
    [SerializeField] private GameObject startRoom;
    [SerializeField] private GameObject defaultRoom;
    
    private GameObject[,] rooms;
    [SerializeField] private int roomWidth;
    [SerializeField] private int roomHeight;
    [SerializeField] private int roomCount;

    // For room filling
    [SerializeField] private FillRoomUI fillRoomUI;
    [SerializeField] private List<GameObject> obstacles;
    private int obstaclesForOneRoom;
    [SerializeField] private NavMeshSurface2d navMeshSurface2d;
    
    private Tuple<int, int> currentRoom;
    private GameObject player;
    private new GameObject camera;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camera = GameObject.FindGameObjectWithTag("CameraPoint");
        rooms = new GameObject[roomCount * 2 + 1, roomCount * 2 + 1];
        CreateDungeon(GenerateDungeon(roomCount));
        DisableImpasseDoors();
        
    }

    private bool[,] GenerateDungeon(int roomCount)
    {
        var cells = new bool[roomCount * 2 + 1, roomCount * 2 + 1];
        cells[roomCount, roomCount] = true;
        for (int i = 0; i < roomCount; i++)
        {
            var allEmptyCells = FindAllEmptyCells(cells);
            var newRoomPlace = Random.Range(0, allEmptyCells.Count);
            cells[allEmptyCells[newRoomPlace].Item1, allEmptyCells[newRoomPlace].Item2] = true;
        }

        return cells;
    }

    private List<Tuple<int, int>> FindAllEmptyCells(bool[,] cells)
    {
        var allEmptyCells = new List<Tuple<int, int>>();
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                if (cells[i, j] == true)
                {
                    var emptyNeighbours = GetEmptyNeighbours(cells, i, j);
                    //allEmptyCells.AddRange(emptyNeighbours);
                    for (int k = 0; k < emptyNeighbours.Count; k++)
                    {
                        if (!allEmptyCells.Contains(emptyNeighbours[k]))
                        {
                            allEmptyCells.Add(emptyNeighbours[k]);
                        }
                    }
                }
            }
        }

        return allEmptyCells;
    }

    private List<Tuple<int, int>> GetEmptyNeighbours(bool[,] cells, int i, int j)
    {
        var neighbours = new List<Tuple<int, int>>();

        try
        {
            if (cells[i + 1, j] == false) neighbours.Add(new Tuple<int, int>(i + 1, j));
        }
        catch (IndexOutOfRangeException) { }
        try
        {
            if (cells[i - 1, j] == false) neighbours.Add(new Tuple<int, int>(i - 1, j));

        }
        catch (IndexOutOfRangeException) { }
        try
        {
            if (cells[i, j + 1] == false) neighbours.Add(new Tuple<int, int>(i, j + 1));
        }
        catch (IndexOutOfRangeException) { }
        try
        {
            if (cells[i, j - 1] == false) neighbours.Add(new Tuple<int, int>(i, j - 1));
        }
        catch (IndexOutOfRangeException) { }
        
        return neighbours;
    }

    private void CreateDungeon(bool[,] cells)
    {
        var middleRow = cells.GetLength(0) / 2; 
        var middleColumn = cells.GetLength(1) / 2;
        currentRoom = new Tuple<int, int>(middleRow, middleColumn);
        rooms[middleRow, middleColumn] = Instantiate(startRoom, Vector3.zero, Quaternion.identity, navMeshSurface2d.gameObject.transform);
        rooms[middleRow, middleColumn].GetComponent<Room>().visited = true;
        cells[middleRow, middleColumn] = false;
        
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                if (cells[i, j])
                {
                    rooms[i,j] = Instantiate(defaultRoom,
                        new Vector3(roomWidth * (i - middleRow),
                            roomHeight * (j - middleColumn)),
                        Quaternion.identity, transform);
                }
            }
        }
        
        navMeshSurface2d.BuildNavMesh();
    }

    private void DisableImpasseDoors()
    {
        for (int i = 0; i < rooms.GetLength(0); i++)
        {
            for (int j = 0; j < rooms.GetLength(1); j++)
            {
                var currRoom = rooms[i, j];
                if (currRoom != null)
                {
                    var room = currRoom.GetComponent<Room>();
                    try
                    {
                        if (rooms[i + 1, j] == null) room.doors[2].SetActive(false);
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (rooms[i - 1, j] == null) room.doors[0].SetActive(false);
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (rooms[i, j + 1] == null) room.doors[1].SetActive(false);
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (rooms[i, j - 1] == null) room.doors[3].SetActive(false);
                    }
                    catch (IndexOutOfRangeException) { }
                }                
            }
        }
    }
    
    public void MoveToAnotherRoom(int enteredDoorSide)
    {
        var currRoom = rooms[currentRoom.Item1, currentRoom.Item2];
        var spawnPos = currRoom.transform.position;
        var spawnSide = 0;
        switch (enteredDoorSide)
        {
            case 0:
                spawnPos += new Vector3(-roomWidth, 0, 0);
                currentRoom = new Tuple<int, int>(currentRoom.Item1 - 1, currentRoom.Item2);
                spawnSide = 2;
                break;
            case 1:
                spawnPos += new Vector3(0, roomHeight, 0);
                currentRoom = new Tuple<int, int>(currentRoom.Item1, currentRoom.Item2 + 1);
                spawnSide = 3;
                break;
            case 2:
                spawnPos += new Vector3(roomWidth, 0, 0);
                currentRoom = new Tuple<int, int>(currentRoom.Item1 + 1, currentRoom.Item2);
                spawnSide = 0;
                break;
            case 3:
                spawnPos += new Vector3(0, -roomHeight, 0);
                currentRoom = new Tuple<int, int>(currentRoom.Item1, currentRoom.Item2 - 1);
                spawnSide = 1;
                break;
        }

        currRoom = rooms[currentRoom.Item1, currentRoom.Item2];
        var room = currRoom.GetComponent<Room>();
        camera.transform.position = spawnPos;
        
        
        room.doors[spawnSide].GetComponent<Door>().SpawnPlayer(player);
        
        if (!room.visited)
        {
            player.SetActive(false);
            var rand1 = Random.Range(0, obstacles.Count);
            var rand2 = Random.Range(0, obstacles.Count);
            
            fillRoomUI.gameObject.SetActive(true);
            fillRoomUI.SetNewRoom(obstacles[rand1], obstacles[rand2], currRoom.transform.position);
        }
        else
        {
            
        }
    }

    public void SetRoomObstacle(GameObject obstacle)
    {
        rooms[currentRoom.Item1, currentRoom.Item2].GetComponent<Room>()
            .SetObstacle(obstacle, navMeshSurface2d.gameObject.transform);
    }

    public void SetEnemyGroup(GameObject enemyGroup)
    {
        rooms[currentRoom.Item1, currentRoom.Item2].GetComponent<Room>().SetEnemyGroup(enemyGroup);
        OnRoomSettingEnd();
    }

    private void OnRoomSettingEnd()
    {
        navMeshSurface2d.BuildNavMesh();
        player.SetActive(true);
    }
}
