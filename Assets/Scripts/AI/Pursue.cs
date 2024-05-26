using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static State;

public class PursueState : State
{
    public PursueState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {

        name = STATE.PURSUE;
        agent.speed = 5.0f;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        base.Enter();
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
                nextState = new PatrolState(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
