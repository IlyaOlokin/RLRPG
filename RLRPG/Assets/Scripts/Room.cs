using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> doors;

    public List<int> GetFreeDoorsIndexes()
    {
        var freeDoorsIndexes = new List<int>();
        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i].GetComponent<Door>().doorNextDoor == null)
            {
                freeDoorsIndexes.Add(i);
            }
        }

        return freeDoorsIndexes;
    } 
    
    public List<GameObject> GetFreeDoors()
    {
        var freeDoorsIndexes = new List<GameObject>();
        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i].GetComponent<Door>().doorNextDoor == null)
            {
                freeDoorsIndexes.Add(doors[i]);
            }
        }

        return freeDoorsIndexes;
    } 
}
