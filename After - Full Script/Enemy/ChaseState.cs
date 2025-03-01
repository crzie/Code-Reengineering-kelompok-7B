using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    private Enemy enemy;
    private NavMeshAgent agent;
    private Player player;

    private float chaseTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        agent = animator.GetComponent<NavMeshAgent>();
        player = Player.Instance;

        agent.speed = enemy.BaseSpeed * enemy.ChaseSpeedModifier;
        agent.angularSpeed = enemy.RotationSpeed * 2000;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chaseTimer += Time.deltaTime;
        if (chaseTimer < enemy.ChaseCooldown) return;

        chaseTimer -= enemy.ChaseCooldown;

        agent.SetDestination(player.transform.position);
        float distance = Vector3.Distance(player.transform.position, animator.transform.position);
        if (distance > enemy.AggroRange)
        {
            animator.SetBool("isChasing", false);
        }
        else if (distance < enemy.AttackRange && !enemy.AttackInCooldown)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isChasing", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
