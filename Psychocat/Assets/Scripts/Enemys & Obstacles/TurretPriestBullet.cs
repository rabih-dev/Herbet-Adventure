using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPriestBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int atkDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Singleton.GetInstance.playerCombat.TakeDamage(atkDamage);        
        }
    }
}
