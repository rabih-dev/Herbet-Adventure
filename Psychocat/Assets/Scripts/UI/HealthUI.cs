using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI: MonoBehaviour
{
    [SerializeField]private GameObject[] hearts = new GameObject[3];

    public void UpdateHealth(int curPlayerHealth)
    {
        switch (curPlayerHealth)
        {

            case 3:

                Debug.Log("Health Updated Without Tooking Damage");

                break;

            case 2:

                hearts[2].SetActive(false);

                break;

            case 1:
                hearts[1].SetActive(false);
                break;

            case 0:
                hearts[0].SetActive(false);
                break;

            default:
                Debug.Log("Health below zero");
                break;
        }
    }
}
