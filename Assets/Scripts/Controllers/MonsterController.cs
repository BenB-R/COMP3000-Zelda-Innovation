using UnityEngine;
using UnityEngine.AI;

namespace fyp
{
    public class MonsterController : AnimalController
    {
        [Header("Monster Stats")]
        [SerializeField] private float attackDamage = 20f;
        [SerializeField] private float detectionRange = 15f;
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private float meleeRange = 2.0f;

        private float lastAttackTime = 0;
        private Transform playerTransform;

        protected void Start()
        {
            base.Start(); // Call the start method of the base class
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        protected void Update()
        {
            base.Update(); // Call the update method of the base class

            if (Vector3.Distance(transform.position, playerTransform.position) <= detectionRange)
            {
                agent.SetDestination(playerTransform.position);
                animator.SetBool("IsMoving", true);

                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack();
                }
            }
        }

        private void Attack()
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");

            Debug.Log("Attacking");

            if (Vector3.Distance(transform.position, playerTransform.position) <= meleeRange)
            {
                Debug.Log("Attack successful");
                playerTransform.GetComponent<PlayerManager>().TakeDamage(attackDamage);
            }
        }
    }
}
