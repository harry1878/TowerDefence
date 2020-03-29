using UnityEngine;

public class MissileTurret : TurretStatus
{
    protected override void Shoot()
    {
        if (target == null) return;
        Bullet e = Instantiate(Bullet,
            BulletPoint.position,
            Quaternion.identity,
            null).GetComponent<Bullet>();
        e.SetTarget(target, Damage, BulletSpeed);

    }
}

