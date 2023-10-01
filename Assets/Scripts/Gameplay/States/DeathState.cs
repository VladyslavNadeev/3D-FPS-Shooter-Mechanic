public class DeathState : EnemyStateBase
{
    public DeathState(bool needsExitTime, Enemy Enemy) : base(needsExitTime, Enemy) { }
    
    public override void OnEnter()
    {
        base.OnEnter();
        Agent.isStopped = true;
        Animator.Play("Death");
    }
}