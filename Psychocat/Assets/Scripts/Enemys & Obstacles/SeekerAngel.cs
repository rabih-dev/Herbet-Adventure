using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(EnemyCombat))]
public class SeekerAngel : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private float speed;
    [SerializeField] private float nextWayPointDistance;
    [SerializeField] private bool lookAhead;
    [SerializeField] private float visionRange; //add campo de visao e patrol enquanto fora dele
        
    
    private int currentWaypoint;
    
    private Path path;
    private Seeker sk;
    private Rigidbody2D rb;
    private EnemyCombat enemyCombat;
    private Vector2 directionedForce;

    void Start()
    {
        currentWaypoint = 0;
        rb = GetComponent<Rigidbody2D>();
        sk = GetComponent<Seeker>();
        enemyCombat = GetComponent<EnemyCombat>();            
        InvokeRepeating("UpdatePath",1f, 0.5f);
    }
   
    void Update()
    {
        if (path == null)
        {
            return;
        }
        
            FollowPlayer();
            LookAtPlayer();
    }

    void UpdatePath()
    {
        if (sk.IsDone())
        {
            sk.StartPath(transform.position, playerPos.position, OnPathComplete);
        }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        directionedForce = direction * speed * Time.deltaTime;

        rb.AddForce(directionedForce);

        if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }

    void LookAtPlayer()
    {
        if (lookAhead)
        {
            if (directionedForce.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            else if (directionedForce.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {       
        if (collision.gameObject.CompareTag("Player"))
        {
            Singleton.GetInstance.playerCombat.TakeDamage(enemyCombat.atkDmg);
        }
    }
}
