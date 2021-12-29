using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    [SerializeField] private int atkDmg;
    [SerializeField] private float atkRange;
    [SerializeField] private float atkCooldown;
    [SerializeField] private Transform atkPoint;
    private bool canAttack;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private SpriteRenderer playerSprite;

    Rigidbody2D rb;
    [SerializeField] private Vector2 knockbackForce;

    [SerializeField] private float invulnerabilityTime;
    [SerializeField] private float hitStunTime;

    void Start()
    {
        canAttack = true;
        currentHealth = maxHealth;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && canAttack)
        {
            Attack();
            StartCoroutine(AttackCooldown());
        }
    }

    void Attack()
    {       
       Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(atkPoint.position, atkRange, enemyMask);

        foreach (Collider2D enemy in hitEnemys)
        {
            enemy.gameObject.GetComponent<EnemyCombat>().TakeDamage(atkDmg);        
        }
        
    }

    IEnumerator AttackCooldown() 
    {
        StartCoroutine(Singleton.GetInstance.playerMovement.StoppingMovement(atkCooldown));
        canAttack = false;
        playerSprite.color = Color.blue;
        yield return new WaitForSeconds(atkCooldown);
        canAttack = true;
        playerSprite.color = Color.white;
    }

        public void TakeDamage(int dmg)
    {        
        rb.AddForce(new Vector2(knockbackForce.x * -transform.localScale.x, knockbackForce.y));
        StartCoroutine(BecomingInvulnerable());
        StartCoroutine(StunningPlayer(hitStunTime));
        currentHealth -= dmg;
        Singleton.GetInstance.healthUI.UpdateHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        Debug.Log("player mortinho");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {          
            TakeDamage(collision.gameObject.GetComponent<EnemyCombat>().atkDmg);
        }
    }

    IEnumerator BecomingInvulnerable()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        playerSprite.color = Color.magenta;
        yield return new WaitForSeconds(invulnerabilityTime);
        Physics2D.IgnoreLayerCollision(8, 9, false);
        playerSprite.color = Color.white;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(atkPoint.position, atkRange);
    }

    IEnumerator StunningPlayer(float stunTime)
    {
       canAttack = false;
       StartCoroutine(Singleton.GetInstance.playerMovement.StoppingMovement(stunTime));
       yield return new WaitForSeconds(stunTime);
       canAttack = true;
    }
  
}
