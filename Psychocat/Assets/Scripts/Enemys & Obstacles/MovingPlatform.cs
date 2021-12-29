using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] private float speed;
    
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    private Vector3 nextPos;
  
    void Start()
    {
        nextPos = startPos.position; 
    }

    // Update is called once per frame
    void Update()
    {
        CheckingDirection();
        Moving();
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
        }

        else if (transform.position == endPos.position)
        {
            nextPos = startPos.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPos.position, endPos.position);
    }

}
