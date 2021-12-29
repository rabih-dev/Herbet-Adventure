using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
public class GetDownPlatform : MonoBehaviour
{    
    private PlatformEffector2D platEffector;

    void Start()
    {
        platEffector = GetComponent<PlatformEffector2D>();   
    }

    // Update is called once per frame
    void Update()
    {
     
    }


       

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                platEffector.surfaceArc = -180;
                Invoke("BackToNormal", 0.2f);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                platEffector.surfaceArc = -180;
                Invoke("BackToNormal", 0.3f);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            platEffector.surfaceArc = 180;
        }
    }
}
