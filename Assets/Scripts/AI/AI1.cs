using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine2 : MonoBehaviour
{
    public enum EnemyState
    {
        Attack,
        Move
    }

    public GameObject[] movePositions; // Array to store move positions
    public float moveSpeed = 5f; // Speed at which the enemy moves
    public GameObject player;
    public float attackRange;
    public GameObject bulletPrefab;
    public float bulletSpeed;

    private EnemyState currentState = EnemyState.Move; // Initial state
    private NavMeshAgent navMeshAgent;
    private int currentPositionIndex = 0; // Index of the current move position
    private float attackCooldown = 10f; // Cooldown for attacking
    private float attackTimer = 0f; // Timer for attacking

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found!");
        }
        else
        {
            navMeshAgent.speed = moveSpeed;
        }
    }

    private void Update()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            case EnemyState.Attack:
                AttackState();
                break;
            case EnemyState.Move:
                MoveState();
                break;
        }
    }

    private void AttackState()
    {
        Debug.Log("Attacking...");
        // Rotate towards the player
        Vector3 direction = player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        ShootBullet(direction);

        // Transition to Move state after cooldown
        navMeshAgent.SetDestination(player.transform.position);
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            currentState = EnemyState.Move;
            attackTimer = 0f;
        }
    }

    private void ShootBullet(Vector3 direction)
    {
        // Instantiate a bullet prefab and set its direction
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = direction.normalized * bulletSpeed;
    }

    private void MoveState()
    {
        // Check if there are move positions
        if (movePositions.Length == 0)
        {
            Debug.LogError("No move positions assigned!");
            return;
        }

        // Move towards the current position
        Transform target = movePositions[currentPositionIndex].transform;
        navMeshAgent.SetDestination(target.position);

        // Check if reached the current position
        if (Vector3.Distance(transform.position, target.position) < 5f)
        {
            // Move to the next position
            currentPositionIndex = (currentPositionIndex + 1) % movePositions.Length;
        }

        // Check if player is within attack range and in line of sight
        if (player != null && Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Player2"))
                {
                    currentState = EnemyState.Attack;
                    return;
                }
            }
        }
    }
}