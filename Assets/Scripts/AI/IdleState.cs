using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    private GameObject[] patrolPoints;

    public IdleState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, GameObject[] _patrolPoints)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
        patrolPoints = _patrolPoints;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            Debug.Log("Can see player");
            nextState = new PursueState(npc, agent, anim, player, patrolPoints); // Pass patrol points
            stage = EVENT.EXIT;
        }
        else if (Random.Range(0, 100) < 100)
        {
            nextState = new PatrolState(npc, agent, anim, player, patrolPoints); // Pass patrol points
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
