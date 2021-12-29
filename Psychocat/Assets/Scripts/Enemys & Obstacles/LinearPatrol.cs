using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearPatrol : MonoBehaviour
{
    [SerializeField] private float speed;
    
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    private Vector3 nextPos;

    [Header("startPos must have the x smaller than endPos if u are going use this")]
    [SerializeField] private bool lookAhead;

    void Start()
    {
        nextPos = startPos.position;
    }

    void Update()
    {
        Moving();
        CheckingDirection();
    }

    void Moving()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    void CheckingDirection()
    {
        if (transform.position == startPos.position)
        {
            nextPos = endPos.position;
            if (lookAhead)
            {               
                Invoke("Flip", 0.01f);
            }
        }

        else if (transform.position == endPos.position)
        {
            nextPos = startPos.position;
            if (lookAhead)
            {               
                Invoke("Flip", 0.02f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPos.position, endPos.position);
    }

    void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }
}
