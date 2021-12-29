using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPriest : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootingDelay;
    [SerializeField] private Transform shootingPoint;
    private bool canShoot;
    void Start()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        Shooting();
    }

    void Shooting()
    {
        if (canShoot)
        {
            GameObject bullet = ObjectPooler.instance.SpawnFromPool("TurretPriestBullet", shootingPoint.position, Quaternion.identity);
            bullet.transform.SetParent(this.transform);
            StartCoroutine(ShootingCooldown(shootingDelay));
        }
    }

    IEnumerator ShootingCooldown(float cooldown)
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }
}
