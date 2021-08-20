using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    protected NavMeshAgent navMeshAgent;

    [SerializeField] private HpSlider hpSlider;
    [SerializeField] private float hpMax;
    [SerializeField]private float hp;

    [SerializeField] private float collisionDamage;

    [SerializeField] private float agrDistance;
    [SerializeField] protected float speed;
    protected bool needToBeAggred;
    protected bool aggred;
    protected float distToPlayer;
    
    [SerializeField] protected float idleMoveRadius;
    [SerializeField] protected float idleSpeed;
    [SerializeField] protected float idleMoveCd;
    float idleMoveCdCurrent;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");
        hp = hpMax;
        
        hpSlider.SetMaxValue(hpMax);
        
        OnSpawn();
    }

    protected virtual void OnSpawn()
    {
        
    }
    void Update()
    {
        if (player == null) return;
        distToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distToPlayer > agrDistance && aggred)
        {
            StartCoroutine(ForgetTarget(2f));
        }
        
        if (needToBeAggred && (distToPlayer <= agrDistance || aggred))
        {
            AggredBehaviour();
        }
        else
        {
            IdleBehaviour();
        }
    }

    private void OnDisable()
    {
        hpSlider.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        hpSlider.gameObject.SetActive(true);
    }

    protected virtual void AggredBehaviour()
    {
        navMeshAgent.speed = speed;
        navMeshAgent.SetDestination(player.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnCollision(other);
    }

    protected virtual void OnCollision(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(collisionDamage);
        }
    }

    public virtual void TakeDamage(float dmg)
    {
        hp -= dmg;
        hpSlider.TakeDmg(dmg);
        if (needToBeAggred && !aggred)
        {
            aggred = true;
            StopAllCoroutines();
            StartCoroutine(ForgetTarget(2f));
        }
        if (hp <= 0)
        {
            OnDeath();
        }
    }

    protected IEnumerator ForgetTarget(float delay)
    {
        yield return new WaitForSeconds(delay);
        aggred = false;
    }

    protected virtual void OnDeath()
    {
        Destroy(hpSlider.gameObject);
        Destroy(gameObject);
    }

    protected virtual void IdleBehaviour()
    {
        navMeshAgent.speed = idleSpeed;
        idleMoveCdCurrent -= Time.deltaTime;
        if (idleMoveCdCurrent <= 0)
        {
            navMeshAgent.SetDestination(PickRandomPoint(idleMoveRadius));
            idleMoveCdCurrent = Random.Range(idleMoveCd - 1, idleSpeed + 1);
        }
    }
    
    private Vector3 PickRandomPoint(float radius)
    {
        var x = Random.Range(-1f, 1f);
        var y = Random.Range(-1f, 1f);
        Vector3 v = new Vector3(x, y, 0);
        v = v.normalized * Random.Range(0, radius);
       
        return new Vector3(transform.position.x + v.x, transform.position.y + v.y, 0);
    }
}
