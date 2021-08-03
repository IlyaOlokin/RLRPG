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
    public float spreading;
    
    // Rifle

    // Shotgun
    
    // Laser Gun
    public bool individualAngles;
    public bool showBulletAngles;
    public List<float> bulletAngles;
    public float angleBetweenBullets;

    public GameObject bullet;
    
    [NonSerialized]
    public Transform SourceTransform;
    
    public void Shoot()
    {
        weapon.Shoot(SourceTransform, 
            spreading, 
            bullet, 
            bulletCount, 
            shotCount, 
            individualAngles, 
            bulletAngles, 
            angleBetweenBullets);
    }
    public enum WeaponType
    {
        Rifle,
        Shotgun,
        PlasmaGun
    }
}