using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace fyp
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AnimalController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator animator;
        private AudioSource audioSource;

        [SerializeField] private float health = 100f;
        [SerializeField] private int baseXP = 50;

        [SerializeField] private AudioClip movingSFX;
        [SerializeField] private AudioClip damageSFX;
        [SerializeField] private AudioClip deathSFX;

        public float wanderRadius = 10f;
        public float wanderTimer = 5f;

        private float timer;
        private Vector3 lastPosition;
        private Coroutine wanderCoroutine;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            timer = wanderTimer;
            wanderCoroutine = StartCoroutine(Wander());
            lastPosition = transform.position;
        }

        void Update()
        {
            bool isMoving = Vector3.Distance(transform.position, lastPosition) > 0.01f;
            animator.SetBool("IsMoving", isMoving);
            if (isMoving && !audioSource.isPlaying && movingSFX)
            {
                audioSource.PlayOneShot(movingSFX);
            }
            lastPosition = transform.position;
        }

        private IEnumerator Wander()
        {
            while (true)
            {
                timer += Time.deltaTime;

                if (timer >= wanderTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                }

                yield return null;
            }
        }

        public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;
            randDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
            return navHit.position;
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
            animator.SetBool("IsHit", true);
            StartCoroutine(ResetIsHit());
            if (damageSFX)
            {
                audioSource.PlayOneShot(damageSFX);
            }
            if (health <= 0)
            {
                Die();
            }
        }

        private IEnumerator ResetIsHit()
        {
            yield return null; // Wait one frame
            animator.SetBool("IsHit", false);
        }

        private void Die()
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsDead", true);
            agent.enabled = false;
            if (wanderCoroutine != null)
            {
                StopCoroutine(wanderCoroutine);
            }
            DropXP();
            if (deathSFX)
            {
                audioSource.PlayOneShot(deathSFX);
            }
            Destroy(gameObject, 20f);
        }

        private void DropXP()
        {
            float xpToDrop = Random.Range(baseXP * 0.8f, baseXP * 1.2f);
            Debug.Log($"Dropped {xpToDrop} XP");
        }
    }
}
