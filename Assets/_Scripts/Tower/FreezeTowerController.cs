using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTowerController : BaseTowerController
{
    private float bulletSpeed = 1.0f;

    public override IEnumerator Shoot(Transform target)
    {
        GameObject projectile = Instantiate(projectilePrefab);
        yield return projectile;
    }
}
