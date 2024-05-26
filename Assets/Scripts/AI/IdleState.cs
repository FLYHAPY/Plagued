using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{

    public IdleState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) 
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter() {
        base.Enter();
    }
    public override void Update() {
        if(CanSeePlayer()) {
            Debug.Log("Can see plauer");
            nextState = new PursueState(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

        else if (Random.Range(0, 100) < 100)
        {
            nextState = new PatrolState(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        
    }

    public override void Exit() {
        base.Exit();
    }
}


