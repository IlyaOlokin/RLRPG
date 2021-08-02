using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon
{
    public abstract float DmgMultiplier { get; }
    public abstract float SizeMultiplier { get; }
    public abstract float SpeedMultiplier { get; }
    
    public abstract void Shoot(Weapon weapon);
}

class Rifle : AbstractWeapon
{
    public override float DmgMultiplier => PlayerStats.RifleDmgMultiplier;
    public override float SizeMultiplier => PlayerStats.RifleBulletSizeMultiplier;
    public override float SpeedMultiplier => PlayerStats.RifleBulletSpeedMultiplier;
    
    public override void Shoot(Weapon weapon)
    {
        var playerTransformRotation = weapon.SourceTransform.eulerAngles;
        var bulletAngle = UnityEngine.Random.Range(-weapon.rifleSpreading, weapon.rifleSpreading);
        weapon.MakeAShot(Quaternion.Euler(0, 0,  bulletAngle + playerTransformRotation.z));
    }
}

class Shotgun : AbstractWeapon
{
    public override float DmgMultiplier => PlayerStats.ShotgunDmgMultiplier;
    public override float SizeMultiplier => PlayerStats.ShotgunBulletSizeMultiplier;
    public override float SpeedMultiplier => PlayerStats.ShotgunBulletSpeedMultiplier;
    
    public override void Shoot(Weapon weapon)
    {
        for (int i = 0; i < weapon.shotCount; i++)
        {
            var playerTransformRotation = weapon.SourceTransform.eulerAngles;
            var bulletAngle = UnityEngine.Random.Range(-weapon.shotgunSpreading / 2f, weapon.shotgunSpreading / 2f);

            weapon.MakeAShot(Quaternion.Euler(0, 0,  bulletAngle + playerTransformRotation.z));
        }
    }
}

class PlasmaGun : AbstractWeapon
{
    public override float DmgMultiplier => PlayerStats.PlasmaGunDmgMultiplier;
    public override float SizeMultiplier => PlayerStats.PlasmaGunBulletSizeMultiplier;
    public override float SpeedMultiplier => PlayerStats.PlasmaGunBulletSpeedMultiplier;
    
    public override void Shoot(Weapon weapon)
    {
        if (weapon.shotCount == 1)
        {
            weapon.MakeAShot(weapon.SourceTransform.rotation);

            return;
        }
        for (int i = 1; i <= weapon.shotCount; i++)
        {
            var playerTransformRotation = weapon.SourceTransform.eulerAngles;
            var bulletAngle = 0f;

            if (weapon.individualAngles)
            {
                bulletAngle = weapon.bulletAngles[i - 1];
                weapon.MakeAShot(Quaternion.Euler(0, 0, bulletAngle + playerTransformRotation.z));

            }
            else
            {
                if (weapon.shotCount % 2 == 1)
                {
                    bulletAngle = i / 2 * weapon.angleBetweenBullets * (i % 2 == 0 ? 1 : -1f);
                }
                else
                {
                    bulletAngle = i / 2 * weapon.angleBetweenBullets * (i % 2 == 0 ? 1 : -1f) -
                                  weapon.angleBetweenBullets / 2f;
                }

                weapon.MakeAShot(Quaternion.Euler(0, 0, bulletAngle + playerTransformRotation.z));
            }
        }
    }
}