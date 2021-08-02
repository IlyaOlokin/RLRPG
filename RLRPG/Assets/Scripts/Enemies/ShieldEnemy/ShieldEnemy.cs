using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class ShieldEnemy : Enemy
    {
        [SerializeField] private GameObject shieldRotPoint1;
        [SerializeField] private GameObject shieldRotPoint2;
        [SerializeField] private float rotationTime;
        
        
        /*[SerializeField] private bool shieldsOpened;
        [SerializeField] private bool shieldsOpening;
        [SerializeField] private bool shieldsMoving;
        [SerializeField] private bool shieldsClosed;
        [SerializeField] private bool shieldsClosing;*/
        //[SerializeField] private bool charging;
        [SerializeField] private ShieldsState shieldsState;
        
        private enum ShieldsState
        {
            Opened,
            Opening,
            Closed,
            Closing,
            Charging
        }
        
        protected override void OnSpawn()
        {
            needToBeAggred = true;
            //shieldsClosed = true;
            shieldsState = ShieldsState.Opened;

        }

        /*private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                StartCoroutine(OpenShields());
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(CloseShields());
            }
        }*/


        protected override void AggredBehaviour()
        {
            switch (shieldsState)
            {
                case ShieldsState.Opened:
                    base.AggredBehaviour();
                    LookAt(player.transform.position);
                    if (distToPlayer <= 4)
                    {
                        navMeshAgent.isStopped = true;
                        shieldsState = ShieldsState.Closing;
                    }
                    break;
                
                case ShieldsState.Closing:
                    var currentRot11 = shieldRotPoint1.transform.localEulerAngles.z;
                    var currentRot22 = shieldRotPoint2.transform.localEulerAngles.z;

                    var rotReached11 = Math.Abs(currentRot11 - 30) < 1f;
                    if (!rotReached11)
                    {
                        shieldRotPoint1.transform.Rotate(new Vector3(0, 0, 1), -40 * Time.deltaTime);
                    }

                    var rotReached22 = Math.Abs(currentRot22 - 330) < 1f;
                    if (!rotReached22)
                    {
                        shieldRotPoint2.transform.Rotate(new Vector3(0, 0, 1), 40 * Time.deltaTime);
                    }

                    if (rotReached11 && rotReached22)
                    {
                        shieldsState = ShieldsState.Closed;
                    }
                    LookAt(player.transform.position);
                    break;
                
                case ShieldsState.Closed:
                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(player.transform.position + (player.transform.position - transform.position).normalized);
                    navMeshAgent.speed = speed * 6;
                    shieldsState = ShieldsState.Charging;
                    break;
                
                case ShieldsState.Charging:
                    if (DestinationReached())
                    {
                        shieldsState = ShieldsState.Opening;
                        navMeshAgent.isStopped = false;
                    }

                    break;
                
                case ShieldsState.Opening:
                    var currentRot1 = shieldRotPoint1.transform.localEulerAngles.z;
                    var currentRot2 = shieldRotPoint2.transform.localEulerAngles.z;
                    
                    var rotReached1 = Math.Abs(currentRot1 - 90) < 1f;
                    if (!rotReached1)
                    {
                        shieldRotPoint1.transform.Rotate(new Vector3(0, 0, 1), 40 * Time.deltaTime);
                    }

                    var rotReached2 = Math.Abs(currentRot2 - 270) < 1f;
                    if (!rotReached2)
                    {
                        shieldRotPoint2.transform.Rotate(new Vector3(0, 0, 1), -40 * Time.deltaTime);
                    }

                    if (rotReached1 && rotReached2)
                    {
                        shieldsState = ShieldsState.Opened;
                    }
                    
                    break;
            }
        }

        private bool DestinationReached()
        {
            return Vector2.Distance(navMeshAgent.destination, transform.position) < 0.1f;
        }

        private void LookAt(Vector3 target)
        {
            var lookDirection = new Vector2(target.x - transform.position.x, target.y - transform.position.y);
            transform.up = lookDirection;
        }
        
        protected override void IdleBehaviour()
        {
            if (shieldsState == ShieldsState.Opened)
            {
                //navMeshAgent.isStopped = false;
                base.IdleBehaviour();
                
                transform.up = navMeshAgent.velocity;
            }
            else
            {
                AggredBehaviour();
            }
            
        }
    }
}
