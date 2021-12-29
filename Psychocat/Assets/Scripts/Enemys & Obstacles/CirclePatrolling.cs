using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePatrolling : MonoBehaviour
{
    private Vector2 routePos;
    private float timeCounting;

    [SerializeField] private float speed;
    [SerializeField] private Vector2 circleSize;
    private Vector2 posOffset;

    void Start()
    {
        posOffset = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        timeCounting += Time.deltaTime;
        routePos.x = Mathf.Cos(timeCounting * speed) * circleSize.x;
        routePos.y = Mathf.Sin(timeCounting * speed) * circleSize.y;
        transform.position = new Vector3(routePos.x + posOffset.x, routePos.y + posOffset.y, transform.position.z);
    }

}
