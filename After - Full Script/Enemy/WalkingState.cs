using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingState : StateMachineBehaviour
{
    private Enemy enemy;
    private Player player;
    private Vector3 targetPosition;
    private NavMeshAgent agent;
    private float timer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        agent = animator.GetComponent<NavMeshAgent>();
        player = Player.Instance;
        timer = 0;

        Vector2 randomPos = Random.insideUnitCircle * enemy.WalkRange;
        targetPosition = new Vector3(enemy.spawnPoint.x, 0, enemy.spawnPoint.y) + 
            new Vector3(randomPos.x, 0, randomPos.y);

        targetPosition.y = animator.transform.position.y;

        agent.speed = enemy.BaseSpeed * enemy.WalkSpeedModifier;
        agent.angularSpeed = enemy.RotationSpeed * 2000;

        agent.SetDestination(targetPosition);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if(agent.remainingDistance <= agent.stoppingDistance || timer > 5)
        { 
            animator.SetBool("isWalking", false);
        }

        float distance = Vector3.Distance(player.transform.position, animator.transform.position);
        if (distance < enemy.AggroRange)
        {
            animator.SetBool("isChasing", true);
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
