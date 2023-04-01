using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon, IProjectileWeapon
{
    public List<BaseStat> Stats { get; set; }
    public Transform ProjectileSpawn { get; set; }
    private Animator animator;
    private const string ARROW_CAST = "Arrow_Cast";
    // reference to arrow prefab
    private Arrow arrow;

    void Start()
    {
        arrow = Resources.Load<Arrow>("Weapons/Projectiles/Arrow");
        animator = GetComponent<Animator>();
    }

    void Update() { }
    public void PerformAttack()
    {
        //Debug.Log(this.name + " Attack!");
        
        animator.SetTrigger(ARROW_CAST);
    }
    public void CastProjectile()
    {
        Arrow arrowInstance = Instantiate(arrow, ProjectileSpawn.position, ProjectileSpawn.rotation);
        arrowInstance.Direction = ProjectileSpawn.forward;  // arrow will face the forward direction of the projectile spawn
    }
}
