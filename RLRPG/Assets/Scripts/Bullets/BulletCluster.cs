using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SocialPlatforms;

public class BulletCluster : MonoBehaviour
{
    public float mainBulletDmgMultiplier;
    public float mainBulletSizeMultiplier;
    public float mainBulletSpeedMultiplier;
    public float clusterBulletDmgMultiplier;
    public float clusterBulletSizeMultiplier;

    public int clusterBulletCount;
    public float angleOffset;
    [Range(0.0f, 360.0f)]
    public float clusterBulletRadius;

    public GameObject BulletPrefab;
    private Bullet bullet;

    void Awake()
    {
        bullet = GetComponent<Bullet>();
        
        bullet.deathActions.Add(ClusterLaunch);
        
        bullet.Dmg *= mainBulletDmgMultiplier;
        bullet.Size *= mainBulletSizeMultiplier;
        bullet.Speed *= mainBulletSpeedMultiplier;
        bullet.transform.GetComponent<Light2D>().pointLightOuterRadius *= mainBulletSizeMultiplier;
        bullet.transform.GetComponent<TrailRenderer>().widthMultiplier = mainBulletSizeMultiplier;
        bullet.transform.GetComponent<TrailRenderer>().time /= mainBulletSpeedMultiplier;
    }

    private void ClusterLaunch()
    {
        for (int i = 1; i <= clusterBulletCount; i++)
        {
            float bulletAngle = 0f;
            float angleBetweenBullets = clusterBulletRadius / clusterBulletCount;
            bulletAngle = i / 2 * angleBetweenBullets * (i % 2 == 0 ? 1 : -1f) - angleBetweenBullets / 2f + angleOffset;

            var bulletQuaternion = Quaternion.Euler(0, 0, bulletAngle + transform.eulerAngles.z);

            GameObject bullet = Instantiate(BulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = bulletQuaternion;

            var bullet1 = bullet.gameObject.GetComponent<Bullet>();

            bullet1.Dmg *= clusterBulletDmgMultiplier;
            bullet1.Size *= clusterBulletSizeMultiplier;
        }
    }
}
