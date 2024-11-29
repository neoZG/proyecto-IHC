// using UnityEngine;
// using UnityEngine.AI;

// public class EnemyAI : MonoBehaviour
// {
//     public Transform player;
//     public NavMeshAgent agent;
//     public LayerMask whatIsGround, whatIsPlayer;

//     // Patroling
//     public Vector3 walkPoint;
//     bool walkPointSet;
//     public float walkPointRange;

//     // Attacking
//     public float timeBetweenAttacks;
//     bool alreadyAttacked;

//     // State
//     public float sightRange;
//     public float attackRange;
//     public bool playerInSightRange, playerInAttackRange;

//     private void Awake()
//     {
//         // Ensure player exists in the scene
//         GameObject playerObject = GameObject.Find("Player");
//         // GameObject playerObject = GameObject.FindWithTag("Player");
//         if (playerObject != null)
//         {
//             player = playerObject.transform;
//         }
//         else
//         {
//             Debug.LogError("Player object not found in the scene. Ensure a GameObject named 'Player' exists.");
//         }

//         // Ensure NavMeshAgent is attached
//         agent = GetComponent<NavMeshAgent>();
//         if (agent == null)
//         {
//             Debug.LogError("NavMeshAgent component not found on this GameObject.");
//         }
//     }

//     private void Update()
//     {
//         // Check for sight and attack range
//         playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
//         playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

//         if (!playerInSightRange && !playerInAttackRange) Patroling();
//         if (playerInSightRange && !playerInAttackRange) ChasePlayer();
//         if (playerInSightRange && playerInAttackRange) AttackPlayer();
//     }

//     private void Patroling()
//     {
//         if (!walkPointSet) SearchWalkPoint();

//         if (walkPointSet)
//         {
//             agent.SetDestination(walkPoint);
//         }

//         Vector3 distanceToWalkPoint = transform.position - walkPoint;

//         // Walkpoint reached
//         if (distanceToWalkPoint.magnitude < 1f)
//         {
//             walkPointSet = false;
//         }
//     }

//     private void SearchWalkPoint()
//     {
//         // Calculate random point in range
//         float randomZ = Random.Range(-walkPointRange, walkPointRange);
//         float randomX = Random.Range(-walkPointRange, walkPointRange);

//         walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

//         // Ensure the walk point is on the ground
//         if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
//         {
//             walkPointSet = true;
//         }
//     }

//     private void ChasePlayer()
//     {
//         if (player != null)
//         {
//             agent.SetDestination(player.position);
//         }
//     }

//     private void AttackPlayer()
//     {
//         // Stop moving
//         agent.SetDestination(transform.position);

//         // Face the player
//         if (player != null)
//         {
//             transform.LookAt(player);
//         }

//         if (!alreadyAttacked)
//         {
//             // Attack code here
//             // Example:
//             // Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
//             // rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
//             // rb.AddForce(transform.up * 8f, ForceMode.Impulse);

//             alreadyAttacked = true;
//             Invoke(nameof(ResetAttack), timeBetweenAttacks);
//         }
//     }

//     private void ResetAttack()
//     {
//         alreadyAttacked = false;
//     }

//     private void OnDrawGizmosSelected()
//     {
//         // Visualize sight and attack ranges in the editor
//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireSphere(transform.position, sightRange);

//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, attackRange);
//     }
// }














// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class RandomMovement : MonoBehaviour
// {
//     public NavMeshAgent agent;
//     public float range = 1.0f; // Small radius to allow slight movement

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//     }

//     void Update()
//     {
//         if (agent.remainingDistance <= agent.stoppingDistance) // Check if the agent has reached its destination
//         {
//             Vector3 point;
//             if (GetPointNearCurrentPosition(out point)) // Get a point near the current position
//             {
//                 Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); // Visualize the target position
//                 agent.SetDestination(point);
//             }
//         }
//     }

//     // Function to get a point near the agent's current position
//     bool GetPointNearCurrentPosition(out Vector3 result)
//     {
//         // Add a slight random offset within the range
//         Vector3 offset = Random.insideUnitSphere * range;
//         Vector3 candidatePoint = transform.position + offset;

//         NavMeshHit hit;
//         // Sample position near the agent's current location
//         if (NavMesh.SamplePosition(candidatePoint, out hit, range, NavMesh.AllAreas))
//         {
//             result = hit.position; // Return the valid point on the NavMesh
//             return true;
//         }

//         result = transform.position; // Default to current position if no valid point is found
//         return false;
//     }
// }










// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class RandomMovement : MonoBehaviour
// {
//     public NavMeshAgent agent;
//     public float range = 1.0f; // Small radius for slight movement
//     public Transform player; // Reference to the player's Transform
//     public float followRange = 5.0f; // Distance within which the agent starts following the player
//     public AudioClip eerieSound; // Reference to the eerie sound clip

//     private bool isChasing = false; // Flag to track if the enemy is chasing the player

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//     }

//     void Update()
//     {
//         float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calculate distance to player

//         if (distanceToPlayer <= followRange) // Check if player is within follow range
//         {
//             FollowPlayer();
//             if (!isChasing)
//             {
//                 isChasing = true; // Start chasing
//                 SoundManager.Instance.PlayEerieSound(eerieSound); // Start playing the eerie sound
//             }
//         }
//         else if (agent.remainingDistance <= agent.stoppingDistance) // Otherwise, continue random movement
//         {
//             Vector3 point;
//             if (GetPointNearCurrentPosition(out point)) // Get a point near the current position
//             {
//                 Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); // Visualize the target position
//                 agent.SetDestination(point);
//             }

//             if (isChasing)
//             {
//                 isChasing = false; // Stop chasing
//                 SoundManager.Instance.StopEerieSound(); // Stop playing the eerie sound
//             }
//         }
//     }

//     void FollowPlayer()
//     {
//         Debug.DrawRay(player.position, Vector3.up * 2, Color.red, 0.1f); // Visualize the player's position
//         agent.SetDestination(player.position); // Set destination to player's position
//     }

//     // Function to get a point near the agent's current position
//     bool GetPointNearCurrentPosition(out Vector3 result)
//     {
//         Vector3 offset = Random.insideUnitSphere * range; // Add a slight random offset
//         Vector3 candidatePoint = transform.position + offset;

//         NavMeshHit hit;
//         // Sample position near the agent's current location
//         if (NavMesh.SamplePosition(candidatePoint, out hit, range, NavMesh.AllAreas))
//         {
//             result = hit.position; // Return the valid point on the NavMesh
//             return true;
//         }

//         result = transform.position; // Default to current position if no valid point is found
//         return false;
//     }
// }



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range = 1.0f; // Small radius for slight movement
    public Transform player; // Reference to the player's Transform
    public float followRange = 5.0f; // Distance within which the agent starts following the player
    public AudioClip eerieSound; // Reference to the eerie sound clip

    private bool isChasing = false; // Flag to track if the enemy is chasing the player

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calculate distance to player

        if (distanceToPlayer <= followRange) // Check if player is within follow range
        {
            FollowPlayer();
            if (!isChasing)
            {
                isChasing = true; // Start chasing
                SoundManager.Instance.PlayEerieSound(eerieSound); // Start playing the eerie sound
            }
        }
        else if (agent.remainingDistance <= agent.stoppingDistance) // Otherwise, continue random movement
        {
            Vector3 point;
            if (GetPointNearCurrentPosition(out point)) // Get a point near the current position
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); // Visualize the target position
                agent.SetDestination(point);
            }

            if (isChasing)
            {
                isChasing = false; // Stop chasing
                SoundManager.Instance.StopEerieSound(); // Stop playing the eerie sound
            }
        }
    }

    void FollowPlayer()
    {
        Debug.DrawRay(player.position, Vector3.up * 2, Color.red, 0.1f); // Visualize the player's position
        agent.SetDestination(player.position); // Set destination to player's position
    }

    // Function to get a point near the agent's current position
    bool GetPointNearCurrentPosition(out Vector3 result)
    {
        Vector3 offset = Random.insideUnitSphere * range; // Add a slight random offset
        Vector3 candidatePoint = transform.position + offset;

        NavMeshHit hit;
        // Sample position near the agent's current location
        if (NavMesh.SamplePosition(candidatePoint, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position; // Return the valid point on the NavMesh
            return true;
        }

        result = transform.position; // Default to current position if no valid point is found
        return false;
    }

    // Trigger detection for when the enemy collides with the player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object is the player
        {
            Debug.Log("Player detected by enemy!");
            GameManager.Instance.PlayerDied();
            // Add logic here for what happens when the enemy touches the player (e.g., player death or restart)
            // For example, call a method in your Player script to handle death:
            // Player.Instance.Die(); // You need to implement this in the Player script.
        }
    }
}
