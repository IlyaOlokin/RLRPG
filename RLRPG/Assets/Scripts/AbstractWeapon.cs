using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon
{
    protected abstract float DmgMultiplier { get; }
    protected abstract float SizeMultiplier { get; }
    protected abstract float SpeedMultiplier { get; }

    public abstract void Shoot(Transform sourceTransform,
        float spreading,
        GameObject bulletPref,
        int bulletCount,
        int shotCount,
        bool individualAngles,
        List<float> bulletAngles,
        float angleBetweenBullets);

    internal void MakeAShot(Vector3 startPosition, Quaternion shotQuaternion, GameObject bulletPref, int bulletCount)
    {
        var startPos = startPosition;
        for (var i = 0; i < bulletCount; i++) FireABullet(shotQuaternion, i, startPos, bulletPref, bulletCount);
    }

    private void FireABullet(Quaternion bulletQuaternion, int i, Vector3 startPos, GameObject bulletPref,
        int bulletCount)
    {
        var bullet = Object.Instantiate(bulletPref);
        bullet.transform.rotation = bulletQuaternion;

        var posOffset = bullet.transform.right * ((i + 1) / 2 * (i % 2 == 0 ? 1 : -1));
        const float gapBetweenBullets = 0.4f;

        // ...3 1 0 2 4...
        //  ...3 1 0 2...
        bullet.transform.position = startPos + posOffset * gapBetweenBullets + bullet.transform.up * 0.25f;

        if (bulletCount % 2 == 0) bullet.transform.position += bullet.transform.right * (gapBetweenBullets / 2f);

        var bullet1 = bullet.gameObject.GetComponent<Bullet>();

        bullet1.Dmg *= DmgMultiplier;
        bullet1.Size *= SizeMultiplier;
        bullet1.Speed *= SpeedMultiplier;
    }
}

internal class Rifle : AbstractWeapon
{
    protected override float DmgMultiplier => PlayerStats.RifleDmgMultiplier;
    protected override float SizeMultiplier => PlayerStats.RifleBulletSizeMultiplier;
    protected override float SpeedMultiplier => PlayerStats.RifleBulletSpeedMultiplier;

    public override void Shoot(Transform sourceTransform,
        float spreading,
        GameObject bulletPref,
        int bulletCount,
        int shotCount,
        bool individualAngles,
        List<float> bulletAngles, 
        float angleBetweenBullets)
    {
        var playerTransformRotation = sourceTransform.eulerAngles;
        var bulletAngle = Random.Range(-spreading, spreading);
        MakeAShot(sourceTransform.position,
            Quaternion.Euler(0, 0, bulletAngle + playerTransformRotation.z),
            bulletPref,
            bulletCount);
    }
}

internal class Shotgun : AbstractWeapon
{
    protected override float DmgMultiplier => PlayerStats.ShotgunDmgMultiplier;
    protected override float SizeMultiplier => PlayerStats.ShotgunBulletSizeMultiplier;
    protected override float SpeedMultiplier => PlayerStats.ShotgunBulletSpeedMultiplier;

    public override void Shoot(Transform sourceTransform,
        float spreading,
        GameObject bulletPref,
        int bulletCount,
        int shotCount,
        bool individualAngles,
        List<float> bulletAngles,
        float angleBetweenBullets)
    {
        for (var i = 0; i < shotCount; i++)
        {
            var playerTransformRotation = sourceTransform.eulerAngles;
            var bulletAngle = Random.Range(-spreading / 2f, spreading / 2f);

            MakeAShot(sourceTransform.position,
                Quaternion.Euler(0, 0, bulletAngle + playerTransformRotation.z),
                bulletPref,
                bulletCount);
        }
    }
}

internal class PlasmaGun : AbstractWeapon
{
    protected override float DmgMultiplier => PlayerStats.PlasmaGunDmgMultiplier;
    protected override float SizeMultiplier => PlayerStats.PlasmaGunBulletSizeMultiplier;
    protected override float SpeedMultiplier => PlayerStats.PlasmaGunBulletSpeedMultiplier;

    public override void Shoot(Transform sourceTransform,
        float spreading,
        GameObject bulletPref,
        int bulletCount,
        int shotCount,
        bool individualAngles,
        List<float> bulletAngles,
        float angleBetweenBullets)
    {
        if (shotCount == 1)
        {
            MakeAShot(sourceTransform.position,
                sourceTransform.rotation,
                bulletPref,
                bulletCount);

            return;
        }

        for (var i = 1; i <= shotCount; i++)
        {
            var playerTransformRotation = sourceTransform.eulerAngles;
            var bulletAngle = 0f;

            if (individualAngles)
            {
                bulletAngle = bulletAngles[i - 1];
                MakeAShot(sourceTransform.position,
                    Quaternion.Euler(0, 0, bulletAngle + playerTransformRotation.z),
                    bulletPref,
                    bulletCount);
            }
            else
            {
                if (shotCount % 2 == 1)
                    bulletAngle = i / 2 * angleBetweenBullets * (i % 2 == 0 ? 1 : -1f);
                else
                    bulletAngle = i / 2 * angleBetweenBullets * (i % 2 == 0 ? 1 : -1f) -
                                  angleBetweenBullets / 2f;

                MakeAShot(sourceTransform.position,
                    Quaternion.Euler(0, 0, bulletAngle + playerTransformRotation.z),
                    bulletPref,
                    bulletCount);
            }
        }
    }
}