using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static State;

public class PursueState : State
{
    private GameObject[] patrolPoints; // Store patrol points for transitioning back to PatrolState

    public PursueState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, GameObject[] _patrolPoints)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 5.0f;
        agent.isStopped = false;
        patrolPoints = _patrolPoints; // Assign patrol points
    }

    public override void Enter()
    {
        base.Enter();
        patrolPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
    }

    public override void Update()
    {
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new AttackState(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }

            if (CanSeePlayer())
            {
                agent.SetDestination(player.position);
            }
            else
            {
                // Pass patrol points when transitioning back to PatrolState
                nextState = new PatrolState(npc, agent, anim, player, patrolPoints);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
