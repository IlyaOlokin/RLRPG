using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillRoomUI : MonoBehaviour
{
    
    [SerializeField] private GameObject obstacleChoice;
    [SerializeField] private RoomChoiceButton[] roomChoiceButtons = new RoomChoiceButton[2];
    
    [SerializeField] private GameObject enemyGroupChoice;
    [SerializeField] private EnemyGroupChoiceButton[] enemyGroupChoiceButton = new EnemyGroupChoiceButton[2];
    
    [SerializeField] private GameObject backGround;

    [SerializeField] private RoomManager rm;
    
    public void SetNewRoom(GameObject obstacles1, GameObject obstacles2, Vector2 pos)
    {
        obstacleChoice.SetActive(true);
        backGround.SetActive(true);
        roomChoiceButtons[0].GetNewObject(obstacles1, pos);
        roomChoiceButtons[1].GetNewObject(obstacles2, pos);
    }

    private void SetNewEnemyGroups(List<GameObject> enemyGroups, Vector2 pos)
    {
        enemyGroupChoiceButton[0].GetNewObject(enemyGroups[0], pos);
        enemyGroupChoiceButton[1].GetNewObject(enemyGroups[1], pos);
    }

    public void ChooseObstacle(GameObject obstacle)
    {
        rm.SetRoomObstacle(obstacle);
        obstacleChoice.SetActive(false);
        
        SetNewEnemyGroups(obstacle.GetComponent<Obstacle>().enemyGroups, obstacle.transform.position);
        enemyGroupChoice.SetActive(true);
    }

    public void ChooseEnemyGroup(GameObject enemyGroup)
    {
        rm.SetEnemyGroup(enemyGroup);
        enemyGroupChoice.SetActive(false);
        backGround.SetActive(false);
    }
}
