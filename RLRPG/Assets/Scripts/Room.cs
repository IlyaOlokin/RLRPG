using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> doors;
    public bool visited;

    public GameObject obstacle;

    public void SetObstacle(GameObject obstacle, Transform parent)
    {
        this.obstacle = obstacle;
        visited = true;
        Instantiate(this.obstacle, transform.position, Quaternion.identity, parent);
    }

    public void SetEnemyGroup(GameObject enemyGroup)
    {
        enemyGroup.SetActive(true);
    }
}
