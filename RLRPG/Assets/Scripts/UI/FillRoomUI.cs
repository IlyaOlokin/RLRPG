using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillRoomUI : MonoBehaviour
{
    
    [SerializeField] private GameObject[] roomObstacles = new GameObject[2];
    [SerializeField] private RoomManager rm;

    public void SetNewObstacles(GameObject obstacles1, GameObject obstacles2)
    {
        roomObstacles[0] = obstacles1;
        roomObstacles[1] = obstacles2;
    }

    public void ChooseObstacle(int i)
    {
        rm.SetRoomObstacle(roomObstacles[i]);
        gameObject.SetActive(false);
    }
}
