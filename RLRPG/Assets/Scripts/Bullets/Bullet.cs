using System;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Dmg;
    public float Speed;
    public float Size;
    public List<Action<Collision2D>> collisionActions = new List<Action<Collision2D>>();
    public List<Action> deathActions = new List<Action>();

    private Rigidbody2D rb;

    [NonSerialized] public bool readyToDie;

    public List<Action> startActions = new List<Action>();
    public List<Action> updateActions = new List<Action>();

    [SerializeField]public Vector2 velocityDir;

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        OnCreate();
        readyToDie = true;
        if (collisionActions.Count != 0) readyToDie = false;
        foreach (var act in startActions) act();
    }

    private void Update()
    {
        
    }

    private void OnDeaths()
    {
        foreach (var act in deathActions) act();

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnCollision(other);
    }

    private void OnCreate()
    {
        rb.velocity = transform.up * Speed;

        transform.localScale = new Vector3(transform.localScale.x * Size, transform.localScale.y * Size,
            transform.localScale.z * Size);
    }
    
    private void OnCollision(Collision2D other)
    {
        if (!readyToDie)
            foreach (var act in collisionActions)
                act(other);
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(Dmg);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(Dmg);
        }
        
        if (readyToDie) OnDeaths();
    }
}