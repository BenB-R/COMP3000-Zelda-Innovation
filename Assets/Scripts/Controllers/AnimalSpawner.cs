using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace fyp
{
    public class AnimalSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject animalPrefab; // Assign your animal prefab
        [SerializeField]
        private int numberOfAnimals = 5; // Number of animals to spawn
        [SerializeField]
        private float spawnRadius = 10f; // Maximum spawn radius

        private List<NavMeshAgent> spawnedAnimals = new List<NavMeshAgent>();

        void Start()
        {
            SpawnAnimals();
        }

        void SpawnAnimals()
        {
            for (int i = 0; i < numberOfAnimals; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                Vector3 finalPosition = Vector3.zero;

                // Find a point on the NavMesh within spawnRadius
                if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, 1))
                {
                    finalPosition = hit.position;
                }
                else
                {
                    Debug.LogWarning("Could not find position on NavMesh!");
                    continue;
                }

                // Instantiate the animal and set it up
                GameObject spawnedAnimal = Instantiate(animalPrefab, finalPosition, Quaternion.identity);
                NavMeshAgent agent = spawnedAnimal.GetComponent<NavMeshAgent>();

                if (agent != null)
                {
                    spawnedAnimals.Add(agent);
                    StartCoroutine(Wander(agent)); // Start wandering
                }
            }
        }

        IEnumerator<WaitForSeconds> Wander(NavMeshAgent agent)
        {
            while (true)
            {
                Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
                randomDirection += transform.position;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, 1))
                {
                    agent.SetDestination(hit.position);
                }

                yield return new WaitForSeconds(Random.Range(5, 10)); // wait random time before next wander
            }
        }
    }
}
