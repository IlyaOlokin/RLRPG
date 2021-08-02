using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletToxic : MonoBehaviour
{
    public GameObject ToxicPuddlePrefab;
    private Bullet bullet;

    
    

    void Awake()
    {
        bullet = GetComponent<Bullet>();
        
        bullet.deathActions.Add(SpawnToxicPuddle);
    }

    private void SpawnToxicPuddle()
    {
        GameObject puddle = Instantiate(ToxicPuddlePrefab);
        puddle.transform.position = transform.position;
        puddle.transform.localScale = transform.localScale;
    }
}
