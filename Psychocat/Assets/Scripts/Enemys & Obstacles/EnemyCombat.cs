using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    public int atkDmg;

    void Start()
    {
        maxHealth = currentHealth;
    }

    public void TakeDamage(int atkDamage)
    {
        currentHealth -= atkDamage;

        Debug.Log("ai ai tao batendo no bandidinho");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
