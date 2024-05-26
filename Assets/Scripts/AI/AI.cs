using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using static State;

public class AI : MonoBehaviour {

    NavMeshAgent agent;
    Animator anim;
    State currentState;
    bool tookdamage;

    public Transform player;

    void Start() {

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new IdleState(gameObject, agent, anim, player);
    }


    void Update() {

        currentState = currentState.Process();
    }
}
