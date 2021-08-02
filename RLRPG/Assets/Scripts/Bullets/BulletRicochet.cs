using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRicochet : MonoBehaviour
{
    public int RicochetsAvailible;
    
    private Bullet bullet;
    private Rigidbody2D rb;
    private Vector3 lastVelocity;

    private void Awake()
    {
        bullet = GetComponent<Bullet>();
        rb = GetComponent<Rigidbody2D>();
        bullet.collisionActions.Add(Ricochet);
    }

    private void Update()
    {
        lastVelocity = rb.velocity;
    }

    private void Ricochet(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("EnemyShield"))
        {
            if (RicochetsAvailible-- == 0 || other.contacts.Length > 1)
            {
                bullet.readyToDie = true;
                
                return;
            }
            
            var velocity = lastVelocity;
            var normal = new Vector2();
            
            
            for (int i = 0; i < other.contacts.Length; i++)
            {
                normal += other.contacts[i].normal;
            }

            var newDir = Vector2.Reflect(velocity, normal.normalized);

            rb.velocity = newDir;
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,0,angle - 90);
           
        }
        else
        {
            bullet.readyToDie = true;
        }
    }
}
