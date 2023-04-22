using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArrowTowerController : BaseTowerController
{
    [SerializeField] float bulletSpeed = 30f;

    public override IEnumerator Shoot(Transform target)
    {
        isShooting = true;
        yield return new WaitForSeconds(1.5f);

        while (isShooting)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.GetComponent<TowerProjectile>().InitializeArrow(target, tls.baseFireRate, tls.baseDamage);
            projectile.transform.position = shootingPoint.position;
            projectile.transform.rotation = shootingPoint.rotation;
            yield return new WaitForSeconds(tls.baseFireRate);
        }
        yield return null;
    }

}

