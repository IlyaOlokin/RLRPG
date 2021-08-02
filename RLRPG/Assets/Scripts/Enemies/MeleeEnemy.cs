using UnityEngine;

namespace Enemies
{
    public class MeleeEnemy : Enemy
    {
        protected override void OnSpawn()
        {
            needToBeAggred = true;
        }
    }
}
