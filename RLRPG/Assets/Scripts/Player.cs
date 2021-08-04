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
    [SerializeField] private float dashLenght;
    [SerializeField] private float dashSpeed;
    private bool moveAvailable = true;
    private bool isDashing;
    private Vector3 dashPoint;
    private bool isInvincible;
    [SerializeField] private LayerMask rayLayerMask;
    
    private float moveX;
    private float moveY;
    private Rigidbody2D rb;
    private new CircleCollider2D collider;
    private Vector2 lookDirection;

    public WeaponManager weaponManager;
    
    private void Start()
    {
        sliderHp.SetMaxValue(hpMax);
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        hp = hpMax;
    }

    private void Update()
    {
        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        lookDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = lookDirection;

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            Dash();
        }

        if (isDashing)
        {
            MakeDash(dashPoint);
        }
    }

    private void Dash()
    {
        var moveDir = new Vector3(moveX, moveY);
        
        if (moveDir == Vector3.zero) moveDir = lookDirection;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir , dashLenght, rayLayerMask);
        
        if (hit.collider != null) StartDash(Vector3.MoveTowards(hit.point, transform.position, 0.25f));
        else StartDash(transform.position + moveDir.normalized * dashLenght);
    }

    private void StartDash(Vector3 endPos)
    {
        dashPoint = endPos;
        isDashing = true;
        collider.enabled = false;
        moveAvailable = false;
        StartCoroutine(ReturnMoveAbility(Vector3.Distance(endPos, transform.position) / dashSpeed));
    }

    private void MakeDash(Vector3 endPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, dashSpeed * Time.deltaTime);
    }

    IEnumerator ReturnMoveAbility(float delay)
    {
        yield return new WaitForSeconds(delay);
        moveAvailable = true;
        collider.enabled = true;
        isDashing = false;
    }

    private void FixedUpdate()
    {
        if (moveAvailable) Move();
    }

    private void Move()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(moveX * speed, moveY * speed);
    }

    public void OnDeath()
    {
        Destroy(sliderHp.gameObject);
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

