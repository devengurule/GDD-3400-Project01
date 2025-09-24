using Unity.VisualScripting;
using UnityEngine;

namespace GDD3400.Project1
{
    public class SpawnRandomSpheres : MonoBehaviour
    {
        [SerializeField] GameObject spherePrefab;
        [SerializeField] int maxSpheres;
        [SerializeField] float cooldown;

        private Vector3 spawnPosition = new Vector3(0,2.5f,0);
        private float counter = 0f;
        private float sphereCounter = 0f;

        public void Start()
        {
            counter = cooldown;
        }

        public void Update()
        {
            spawnSphere();
        }

        public void spawnSphere()
        {
            if (Cooldown() && sphereCounter < maxSpheres)
            {
                GameObject newSphere = Instantiate(spherePrefab, NewPosition(), Quaternion.identity);
                sphereCounter += 1;
            }
        }

        public bool Cooldown()
        {
            if(counter <= 0)
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

        public Vector3 NewPosition()
        {
            spawnPosition.x = Random.Range(-24.5f, 24.5f);
            spawnPosition.z = Random.Range(-24.5f, 24.5f);

            return spawnPosition;
        }
    }
}
