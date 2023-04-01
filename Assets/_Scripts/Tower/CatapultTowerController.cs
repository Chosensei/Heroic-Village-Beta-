using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultTowerController : BaseTowerController
{
    private float bulletSpeed = 0.5f;
    public override IEnumerator Shoot(Transform target)
    {
        isShooting = true;

        while (isShooting)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = shootingPoint.position;
            projectile.transform.rotation = shootingPoint.rotation;
            projectile.GetComponent<TowerProjectile>().Initialize(target, bulletSpeed, tls.baseDamage);
            yield return new WaitForSeconds(tls.baseFireRate);
        }
        yield return null;
    }
}
