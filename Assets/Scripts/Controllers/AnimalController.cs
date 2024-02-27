using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace fyp
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AnimalController : MonoBehaviour
    {
        private NavMeshAgent agent;
        public float wanderRadius = 10f; // The radius in which the animal will wander.
        public float wanderTimer = 5f; // The time in seconds between random moves.

        private float timer; // Internal timer to track wandering time.

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            timer = wanderTimer; // Initialize the timer.
            StartCoroutine(Wander()); // Start the wandering coroutine.
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        private IEnumerator Wander()
        {
            while (true) // Loop indefinitely
            {
                // Update the timer every frame and check if it's time to wander.
                timer += Time.deltaTime;

                if (timer >= wanderTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0; // Reset the timer.
                }

                yield return null; // Yield until the next frame.
            }
        }

        // Utility method to find a random point on the NavMesh within a certain radius.
        public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;

            randDirection += origin;

            NavMeshHit navHit;

            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

            return navHit.position;
        }
    }
}