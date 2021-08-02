using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    [SerializeField] public float DmgTaken = 0;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            DmgTaken += other.gameObject.GetComponent<Bullet>().Dmg;
        }
    }
}
