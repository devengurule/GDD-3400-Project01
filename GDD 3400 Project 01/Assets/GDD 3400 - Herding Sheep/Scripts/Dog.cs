using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace GDD3400.Project01
{
    public class Dog : MonoBehaviour
    {

        //dog length: 2
        //dog width: 1
        
        private bool _isActive = true;
        public bool IsActive 
        {
            get => _isActive;
            set => _isActive = value;
        }

        // Required Variables (Do not edit!)
        private float _maxSpeed = 5f;
        private float _sightRadius = 7.5f;

        private float targetSpeed;

        // Layers - Set In Project Settings
        public LayerMask _targetsLayer;
        public LayerMask _obstaclesLayer;

        // Tags - Set In Project Settings
        private const string friendTag = "Friend";
        private const string threatTag = "Thread";
        private const string safeZoneTag = "SafeZone";



        // All added variables
        private Vector3 startingPos = Vector3.zero;
        public Vector3 wanderPos = Vector3.zero;
        private bool wandering;
        private Vector3 velocity;
        private Rigidbody rb;
        public float turnRate;
        public float stoppingDistance;
        private float counter;
        public float cooldown;
        private List<Collider> _friendTargets = new List<Collider>();
        private Collider[] _tmpTargets = new Collider[16];

        public void Awake()
        {
            // Find the layers in the project settings
            _targetsLayer = LayerMask.GetMask("Targets");
            _obstaclesLayer = LayerMask.GetMask("Obstacles");
            rb = GetComponent<Rigidbody>();
            wandering = true;
            counter = cooldown;
        }
        private void Start()
        {
            startingPos = transform.position;
        }
        private void Update()
        {
            if (!_isActive) return;
            
            Perception();
            DecisionMaking();
        }
        
        private void Perception()
        {
            //Stolen code from Sheep.cs
            _friendTargets.Clear();

            // Collect all target colliders within the sight radius
            int t = Physics.OverlapSphereNonAlloc(transform.position, _sightRadius, _tmpTargets, _targetsLayer);

            for (int i = 0; i < t; i++)
            {
                var c = _tmpTargets[i];
                if (c == null || c.gameObject == gameObject) continue;

                // Store the friends, threat, and safe zone targets
                switch (c.tag)
                {
                    case friendTag:
                        _friendTargets.Add(c);
                        break;
                }
            }
        }

        private void DecisionMaking()
        {
            CalculateMoveTarget();
        }

        public void CalculateMoveTarget()
        {
            if(_friendTargets.Count > 1)
            {
                //Secondary Movement
                //Head back to start to drop off sheep
                if (Cooldown(cooldown))
                {
                    targetSpeed = 2.5f;
                    wanderPos = startingPos;
                }
            }
            else
            {
                //Primary Movement
                //Wandering
                targetSpeed = _maxSpeed;
            }

            if (!wandering)
            {
                //If wandering is false then get a new position and wandering is true
                //Will happen if reached a target or completely stopped
                wanderPos = NewPosition();
                wandering = true;
            }
        }
        
        /// <summary>
        /// Grabs a new position in the scene
        /// </summary>
        /// <returns></returns>
        public Vector3 NewPosition()
        {
            wanderPos.x = Random.Range(-22.5f, 22.5f);
            wanderPos.z = Random.Range(-22.5f, 22.5f);

            return wanderPos;
        }
        /// <summary>
        /// Timer Cooldown
        /// </summary>
        /// <returns></returns>
        public bool Cooldown(float cooldown)
        {
            if (counter <= 0)
            {
                counter = cooldown;
                return true;
            }
            else
            {
                counter -= Time.deltaTime;
                return false;
            }
        }

        /// <summary>
        /// Make sure to use FixedUpdate for movement with physics based Rigidbody
        /// You can optionally use FixedDeltaTime for movement calculations, but it is not required since fixedupdate is called at a fixed rate
        /// </summary>
        private void FixedUpdate()
        {
            //A lot of stolen code from Sheep.cs
            if (!_isActive) return;
            //sets a target positon
            Vector3 target = new Vector3(wanderPos.x, startingPos.y, wanderPos.z);

            //If we are not at the target position
            if (Vector3.Distance(transform.position, target) > stoppingDistance)
            {
                // Calculate the direction to the target position
                Vector3 direction = (target - transform.position).normalized;

                // Velocity is a target speed unless were at the target position then it gets smaller till nothing
                velocity = direction * Mathf.Min(targetSpeed, Vector3.Distance(transform.position, target));
                rb.linearVelocity = velocity;
            }

            // Calculate the desired rotation towards the movement vector
            if (velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(velocity);

                // Smoothly rotate towards the target rotation based on the turn rate
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnRate);
            }

            //If got to target position or have been stopped get a new target position
            //Or if get close enough to target without seeing anything pick a new target before coming to a complete stop
            if(rb.linearVelocity.magnitude <= 0 || velocity.magnitude <= 0 || Vector3.Distance(transform.position, wanderPos) < _sightRadius/2)
            {
                wandering = false;
            }
        }
    }
}
