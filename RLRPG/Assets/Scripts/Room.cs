using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> doors;
    public bool visited;

    public GameObject obstacle;

    public void SetObstacle(GameObject o, Transform parent)
    {
        obstacle = o;
        visited = true;
        Instantiate(obstacle, transform.position, Quaternion.identity, parent);
    }
}
