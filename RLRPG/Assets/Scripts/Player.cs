using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HpSlider sliderHp;
    private float hp;
    [SerializeField] private float hpMax = 100;
    [SerializeField] private float speed = 5;
    [SerializeField] private float invincibleTime;
    private bool isInvincible;
    
    private float moveX;
    private float moveY;
    private Rigidbody2D rb;
    private Vector2 lookDirection;

    public WeaponManager weaponManager;
    
    private void Start()
    {
        sliderHp.SetMaxValue(hpMax);
        rb = GetComponent<Rigidbody2D>();
        hp = hpMax;
    }

    private void Update()
    {
        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        lookDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = lookDirection;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(moveX * speed, moveY * speed);
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            hp -= damage;
            isInvincible = true;
            StartCoroutine(InvisibleDelay(invincibleTime));
            sliderHp.TakeDmg(damage);
        }
        
        if (hp <= 0) OnDeath();
    }

    private IEnumerator InvisibleDelay(float invincibleTime)
    {
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}

