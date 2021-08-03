using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Weapon weapon = (Weapon) target;
        
        weapon.sprite = (Sprite)EditorGUILayout.ObjectField(weapon.sprite, typeof(Sprite), allowSceneObjects: true);
        weapon.weaponType = (Weapon.WeaponType)EditorGUILayout.EnumPopup("Weapon Type", weapon.weaponType);
        weapon.hasLaserSight = EditorGUILayout.Toggle("Has Laser Sight", weapon.hasLaserSight);
        if (weapon.hasLaserSight)
        {
            weapon.laserSightLenght = EditorGUILayout.FloatField("Laser Sight Lenght", weapon.laserSightLenght);
        }

        switch (weapon.weaponType)
        {
            case Weapon.WeaponType.Rifle:
                weapon.autoShoot = true;
                EditorGUILayout.LabelField("Auto Shooting", weapon.autoShoot.ToString());
                weapon.multiShot = EditorGUILayout.IntField("Shot Count", weapon.multiShot);
                weapon.bulletCount = EditorGUILayout.IntField("Bullet Count", weapon.bulletCount);
                weapon.coolDown = EditorGUILayout.FloatField("Cool Down", weapon.coolDown);
                weapon.spreading = EditorGUILayout.FloatField("Spreading", weapon.spreading);
                break;
            
            case Weapon.WeaponType.Shotgun:
                weapon.autoShoot = false;
                EditorGUILayout.LabelField("Auto Shooting", weapon.autoShoot.ToString());
                weapon.multiShot = EditorGUILayout.IntField("Multi Shot", weapon.multiShot);
                weapon.shotCount = EditorGUILayout.IntField("Shot Count", weapon.shotCount);
                weapon.bulletCount = EditorGUILayout.IntField("Bullet Count", weapon.bulletCount);
                weapon.coolDown = EditorGUILayout.FloatField("Cool Down", weapon.coolDown);
                weapon.spreading = EditorGUILayout.FloatField("Spreading", weapon.spreading);
                break;
            
            case Weapon.WeaponType.PlasmaGun:
                weapon.autoShoot = EditorGUILayout.Toggle("Auto Shooting", weapon.autoShoot);
                EditorGUILayout.Space();
                weapon.multiShot = EditorGUILayout.IntField("Multi Shot", weapon.multiShot);
                weapon.shotCount = EditorGUILayout.IntField("Shot Count", weapon.shotCount);
                weapon.bulletCount = EditorGUILayout.IntField("Bullet Count", weapon.bulletCount);

                if (weapon.shotCount > 1)
                {
                    weapon.individualAngles = EditorGUILayout.Toggle("Individual Bullet Directions", weapon.individualAngles);
                    
                    if (weapon.individualAngles)
                    {
                        weapon.showBulletAngles = EditorGUILayout.Foldout(weapon.showBulletAngles, "Show Bullet Angles", true);
                        if (weapon.showBulletAngles)
                        {
                            List<float> list = weapon.bulletAngles;

                            while(weapon.shotCount > list.Count) list.Add(0);
                            while (weapon.shotCount < list.Count) list.RemoveAt(list.Count - 1);

                            for (int i = 0; i < list.Count; i++)
                            {
                                list[i] = EditorGUILayout.FloatField("Bullet" + i, list[i]);
                            }
                        }
                    }
                    else
                    {
                        weapon.angleBetweenBullets =
                            EditorGUILayout.FloatField("Angle Between Bullets", weapon.angleBetweenBullets);
                    }
                }
                EditorGUILayout.Space();
                weapon.coolDown = EditorGUILayout.FloatField("Cool Down", weapon.coolDown);
                break;
        }
        EditorGUILayout.Space();

        weapon.bullet = (GameObject)EditorGUILayout.ObjectField("Bullet",weapon.bullet, typeof(GameObject));
        EditorUtility.SetDirty(weapon);
    }
    
}
