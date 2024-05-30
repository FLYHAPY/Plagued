using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    int currentIndex = 0; // current waypoint
    public float speed = 1.0f; // Speed of movement
    public GameObject[] points; // Array of points to move between

    // Modified constructor to accept patrol points
    public PatrolState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, GameObject[] _points)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
        points = _points;

        if (points == null || points.Length == 0)
        {
            Debug.LogError("No checkpoints found in PatrolState constructor.");
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        // Null check before using points array
        if (points == null || points.Length == 0)
        {
            Debug.LogError("No checkpoints available in Update()");
            return;
        }

        npc.transform.position = Vector3.MoveTowards(npc.transform.position, points[currentIndex].transform.position, speed * Time.deltaTime);

        // Check if the GameObject has reached the target position
        if (Vector3.Distance(npc.transform.position, points[currentIndex].transform.position) < 0.01f)
        {
            // Update to the next point in the array
            currentIndex = (currentIndex + 1) % points.Length; // Use modulus to wrap around
        }

        if (CanSeePlayer())
        {
            nextState = new PursueState(npc, agent, anim, player, points);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
