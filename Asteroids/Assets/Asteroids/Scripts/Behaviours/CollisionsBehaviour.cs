using UnityEngine;
using System.Collections;

    public class CollisionsBehaviour : RandomizeGameObject
    {
    public static Exploder exploder;

    void Awake()
    {
        if (!exploder)
        {
            exploder = Resources.Load<Exploder>("Exploder");
            exploder.BuildPools();
        }
    }

    #region Hit by Ship/UFO
    protected virtual void OnCollisionEnter(Collision otherCollision)
    {
        GameObject otherObject = otherCollision.gameObject;
        if (otherObject.tag == "Ship")
        {
            HitByShip(otherObject);
        }
    }

    protected void HitByShip(GameObject ship)
    {
        if (!SpaceshipBehaviour.ActiveShield(ship))
        {
            KillWithExplosion(victim: ship);
        }
    }
    #endregion

    #region Hit by Bullet
    protected virtual void OnTriggerEnter(Collider bulletCollider)
    {
        HitByBullet(bulletCollider.gameObject);
    }

    protected virtual void HitByBullet(GameObject bullet)
    {
        RemoveFromGame(bullet);
        KillWithExplosion(victim: this.gameObject);
        Score(destructionScore);
    }

    public void KillWithExplosion(GameObject victim)
    {
        exploder.Explode(victim.tag, victim.transform.position);
        RemoveFromGame(victim);
    }
    #endregion
}