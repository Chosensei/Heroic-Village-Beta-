using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultTowerController : BaseTowerController
{
    //private TowerLevelSwitch tls; 
    private Animator animator;

    public override IEnumerator Shoot(Transform target)
    {
        animator = transform.GetChild(1).GetComponent<Animator>();
        isShooting = true;
        yield return new WaitForSeconds(1.5f);

        // TODO: Handle aniamtions for catapult
        while (isShooting)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.GetComponent<TowerProjectile>().Initialize(target, tls.baseFireRate, tls.baseDamage, tls.baseAoeRadius, tls.baseAoeBlastForce);
            projectile.transform.position = shootingPoint.position;
            projectile.transform.rotation = shootingPoint.rotation;
            animator.SetTrigger("throw");
            animator.speed = tls.baseAnimationSpeed;
            yield return new WaitForSeconds(tls.baseFireRate);
        }
        yield return null;
        animator.ResetTrigger("throw");
    }
}
