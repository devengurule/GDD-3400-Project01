using UnityEngine;

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

        // Layers - Set In Project Settings
        public LayerMask _targetsLayer;
        public LayerMask _obstaclesLayer;

        // Tags - Set In Project Settings
        private string friendTag = "Friend";
        private string threatTag = "Thread";
        private string safeZoneTag = "SafeZone";

        private Vector2 startingPos = Vector2.zero;
        private Vector2 wanderPos = Vector2.zero;
        private Vector2 direction = Vector2.zero;
        [SerializeField] float angularAccel;
        [SerializeField] float accel;
        private Rigidbody rb;

        public void Awake()
        {
            // Find the layers in the project settings
            _targetsLayer = LayerMask.GetMask("Targets");
            _obstaclesLayer = LayerMask.GetMask("Obstacles");
            startingPos = transform.position;

        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            if (!_isActive) return;
            
            Perception();
            DecisionMaking();
        }

        private void Perception()
        {
            
        }

        private void DecisionMaking()
        {
            CalculateMoveTarget();
        }

        public void CalculateMoveTarget()
        {
            //if within sight of sheep
                //target changes to start position
                //speed changes to .5
            //if at target
                //pick new target position
            //move to target
        }

        /// <summary>
        /// Grabs a new position in the scene
        /// </summary>
        /// <returns></returns>
        public Vector3 NewPosition()
        {
            wanderPos.x = Random.Range(-22.5f, 22.5f);
            wanderPos.y = Random.Range(-22.5f, 22.5f);

            return wanderPos;
        }

        /// <summary>
        /// Make sure to use FixedUpdate for movement with physics based Rigidbody
        /// You can optionally use FixedDeltaTime for movement calculations, but it is not required since fixedupdate is called at a fixed rate
        /// </summary>
        private void FixedUpdate()
        {
            if (!_isActive) return;
            
        }







    }
}
