using UnityEngine;

namespace Enemies
{
    public class WeaponEnemy : Enemy
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private Weapon weapon1;
        private float currentCd;
        protected override void OnSpawn()
        {
            needToBeAggred = false;
            
            weapon1 = Instantiate(weapon);
            weapon1.SourceTransform = transform;
            switch ( weapon1.weaponType)
            {
                case Weapon.WeaponType.Rifle:
                    weapon1.weapon = new Rifle();
                    break;
                case Weapon.WeaponType.Shotgun:
                    weapon1.weapon = new Shotgun();
                    break;
                case Weapon.WeaponType.PlasmaGun:
                    weapon1.weapon = new PlasmaGun();
                    break;
            }
        }

        protected override void IdleBehaviour()
        {
            base.IdleBehaviour();
            var lookDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            transform.up = lookDirection;
            currentCd -= Time.deltaTime;
            if (currentCd <= 0)
            {
                currentCd = weapon1.coolDown;
                weapon1.Shoot();
            }
        }
    }
    
}
