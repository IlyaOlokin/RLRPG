using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChoiceButton : MonoBehaviour
{
    [SerializeField] private FillRoomUI fillRoomUI;
    private GameObject _obstacle;

    public void GetNewObject(GameObject obstacle, Vector2 pos)
    {
        _obstacle = Instantiate(obstacle, pos, Quaternion.identity);
        _obstacle.SetActive(false);
    }

    private void OnMouseUp()
    {
        fillRoomUI.ChooseObstacle(_obstacle);
    }

    private void OnMouseOver()
    {
        _obstacle.SetActive(true);
    }
    private void OnMouseExit()
    {
        _obstacle.SetActive(false);
    }
    
    private void OnDisable()
    {
        Destroy(_obstacle);
    }
}
