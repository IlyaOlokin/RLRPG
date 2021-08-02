using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public Sprite sprite;
    public WeaponType weaponType;
    public AbstractWeapon weapon;
    
    public bool autoShoot;
    
    public int multiShot;
    public int shotCount;
    public int bulletCount;
    public float coolDown;
    public bool hasLaserSight;
    public float laserSightLenght;

    // Rifle
    public float rifleSpreading;
    
    // Shotgun
    
    // Laser Gun
    public bool individualAngles;
    public bool showBulletAngles;
    public List<float> bulletAngles;
    public float angleBetweenBullets;
    public float shotgunSpreading;
    
    public GameObject bullet;
    
    [NonSerialized]
    public Transform SourceTransform;
    
    public void Shoot()
    {
        weapon.Shoot(this);
        /*switch (weaponType)
        {
            case WeaponType.Rifle:
                RifleShoot();
                break;
            case WeaponType.Shotgun:
                ShotgunShoot();
                break;
            case WeaponType.PlasmaGun:
                PlasmaGunShoot();
                break;
        }*/
    }

    private void RifleShoot()
    {
        var playerTransformRotation = SourceTransform.eulerAngles;
        var bulletAngle = UnityEngine.Random.Range(-rifleSpreading, rifleSpreading);
        MakeAShot(Quaternion.Euler(0, 0,  bulletAngle + playerTransformRotation.z));

    }

    private void ShotgunShoot()
    {
        for (int i = 0; i < shotCount; i++)
        {
            var playerTransformRotation = SourceTransform.eulerAngles;
            var bulletAngle = UnityEngine.Random.Range(-shotgunSpreading / 2f, shotgunSpreading / 2f);

            MakeAShot(Quaternion.Euler(0, 0,  bulletAngle + playerTransformRotation.z));
        }
        
    }
    private void PlasmaGunShoot()
    {
        if (shotCount == 1)
        {
            MakeAShot(SourceTransform.rotation);

            return;
        }
        for (int i = 1; i <= shotCount; i++)
        {
            var playerTransformRotation = SourceTransform.eulerAngles;
            var bulletAngle = 0f;

            if (individualAngles)
            {
                bulletAngle = bulletAngles[i - 1];
                MakeAShot(Quaternion.Euler(0, 0, bulletAngle + playerTransformRotation.z));

            }
            else
            {
                if (shotCount % 2 == 1)
                {
                    bulletAngle = i / 2 * angleBetweenBullets * (i % 2 == 0 ? 1 : -1f);
                }
                else
                {
                    bulletAngle = i / 2 * angleBetweenBullets * (i % 2 == 0 ? 1 : -1f) - angleBetweenBullets / 2f;
                }

                MakeAShot(Quaternion.Euler(0, 0, bulletAngle + playerTransformRotation.z));
            }
            
        }
    }

    internal void MakeAShot(Quaternion shotQuaternion)
    {
        var startPos = SourceTransform.position;
        for (int i = 0; i < bulletCount; i++)
        {
            FireABullet(shotQuaternion, i, startPos);
        }
    }
    
    private void FireABullet(Quaternion bulletQuaternion, int i, Vector3 startPos)
    {
        var bullet = Instantiate(this.bullet);
        bullet.transform.rotation = bulletQuaternion;

        var posOffset = bullet.transform.right * ((i + 1) / 2 * (i % 2 == 0 ? 1 : -1));
        const float gapBetweenBullets = 0.4f;
        
        // ...3 1 0 2 4...
        //  ...3 1 0 2...
        bullet.transform.position = startPos + posOffset * gapBetweenBullets + bullet.transform.up * 0.25f; 

        if (bulletCount % 2 == 0)
        {
            
            bullet.transform.position += bullet.transform.right * (gapBetweenBullets / 2f);
        }
        
        var bullet1 = bullet.gameObject.GetComponent<Bullet>();
        
        bullet1.Dmg *= weapon.DmgMultiplier;
        bullet1.Size *= weapon.SizeMultiplier;
        bullet1.Speed *= weapon.SpeedMultiplier;

        /*switch (weaponType)
        {
            case WeaponType.Rifle:
                bullet1.Dmg *= PlayerStats.RifleDmgMultiplier;
                bullet1.Size *= PlayerStats.RifleBulletSizeMultiplier;
                bullet1.Speed *= PlayerStats.RifleBulletSpeedMultiplier;
                break;
            case WeaponType.Shotgun:
                bullet1.Dmg *= PlayerStats.ShotgunDmgMultiplier;
                bullet1.Size *= PlayerStats.ShotgunBulletSizeMultiplier;
                bullet1.Speed *= PlayerStats.ShotgunBulletSpeedMultiplier;
                break;
            case  WeaponType.PlasmaGun:
                bullet1.Dmg *= PlayerStats.PlasmaGunDmgMultiplier;
                bullet1.Size *= PlayerStats.PlasmaGunBulletSizeMultiplier;
                bullet1.Speed *= PlayerStats.PlasmaGunBulletSpeedMultiplier;
                break;
        }*/
    }

    public enum WeaponType
    {
        Rifle,
        Shotgun,
        PlasmaGun
    }
}