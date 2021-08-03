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
    }

    public enum WeaponType
    {
        Rifle,
        Shotgun,
        PlasmaGun
    }
}