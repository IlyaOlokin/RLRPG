using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupChoiceButton : MonoBehaviour
{
    [SerializeField] private FillRoomUI fillRoomUI;
    private GameObject _enemyGroup;

    public void GetNewObject(GameObject enemyGroup, Vector2 pos)
    {
        _enemyGroup = Instantiate(enemyGroup, pos, Quaternion.identity);
        _enemyGroup.SetActive(false);
    }

    private void OnMouseUp()
    {
        fillRoomUI.ChooseEnemyGroup(_enemyGroup);
    }

    private void OnMouseOver()
    {
        _enemyGroup.SetActive(true);
    }
    private void OnMouseExit()
    {
        _enemyGroup.SetActive(false);
    }
}
