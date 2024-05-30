using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static State;


public class AttackState : State
{
    AudioSource sound;
    public GameObject[] patrolPoints;
    public AttackState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) 
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;
        sound = npc.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
        base.Enter();
        patrolPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0.0f;

        if (CanAttackPlayer())
        {
            agent.speed = 0;
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2f);
        }
        if (!CanAttackPlayer())
        {
            nextState = new PursueState(npc, agent, anim, player, patrolPoints);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit() 
    {
        base.Exit();
    }
}
