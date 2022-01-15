using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaline : MonoBehaviour
{
    [Header("Randomized Intervals")]
    [SerializeField] private int lowestNumber;
    [SerializeField] private int highestNumber;
    private Transform startPos;

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ScanlineTriggering());
    }

    IEnumerator ScanlineTriggering()
    {
        int interval = Random.Range(lowestNumber, highestNumber);
        yield return new WaitForSeconds(interval);
        animator.SetTrigger("passScanline");
        StartCoroutine(ScanlineTriggering());
    }
}
