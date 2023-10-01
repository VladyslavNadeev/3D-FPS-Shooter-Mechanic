using UnityEngine;

public class ChaseState : EnemyStateBase
{
    private Transform _target;

    public ChaseState(bool needsExitTime, Enemy Enemy, Transform Target) : base(needsExitTime, Enemy)
    {
        _target = Target;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Agent.enabled = true;
        Agent.isStopped = false;
        Animator.Play("ChaseWalking");
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!RequestedExit)
        {
            Agent.SetDestination(_target.position);
        }
        else if(Agent.remainingDistance <= Agent.stoppingDistance)
        {
            fsm.StateCanExit();
        }
    }
}














