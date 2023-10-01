using System;
using System.Collections;
using Infrastructure.Services.PersistenceProgress;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Base Settings")] 
    [SerializeField] private EnemyVitality _enemyVitality;
    [SerializeField] private Animator _enemyAnimator;
    [SerializeField] private NavMeshAgent _enemyAgent;
    [SerializeField] private CapsuleCollider _enemyCollider;

    [Header("Sensors")]
    [SerializeField] private PlayerSensor _followPlayerSensor;
    [SerializeField] private PlayerSensor _rangeAttackPlayerSensor;
    
    [Header("AttackConfig")] 
    [SerializeField] private float _attackCooldown = 2f;

    [Space] 
    
    [Header("DebugInfo")] 
    [SerializeField] private bool _isInChasingRange;

    [SerializeField] private bool _isInAttackingRange;
    [SerializeField] private float _lastAttackTime;


    private StateMachine<EnemyState, StateEvent> _enemyStateMachine;
    private PlayerVitality _playerVitality;
    private IPersistenceProgressService _progress;

    public void Init(PlayerVitality playerVitality, IPersistenceProgressService progress)
    {
        _progress = progress;
        _playerVitality = playerVitality;
        _enemyStateMachine = new();

        _enemyStateMachine.AddState(EnemyState.Idle, new IdleState(false, this));
        _enemyStateMachine.AddState(EnemyState.Chase, new ChaseState(true, this, _playerVitality.transform));
        _enemyStateMachine.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack));
        _enemyStateMachine.AddState(EnemyState.Death, new DeathState(true, this));

        _enemyStateMachine.AddTriggerTransition(
            StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));
        _enemyStateMachine.AddTriggerTransition(
            StateEvent.LosePlayer, new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle));
        
        _enemyStateMachine.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
            (transition) => _isInChasingRange 
                            && Vector3.Distance(_playerVitality.transform.position, transform.position) > _enemyAgent.stoppingDistance)
        );
        
        _enemyStateMachine.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
            (transition) => !_isInChasingRange 
                            || Vector3.Distance(_playerVitality.transform.position, transform.position) <= _enemyAgent.stoppingDistance)
        );
        
        _enemyStateMachine.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldAttack, forceInstantly:true));
        _enemyStateMachine.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldAttack, forceInstantly:true));
        _enemyStateMachine.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, IsNotWithinIdleRange));
        _enemyStateMachine.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));
        
        _enemyStateMachine.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Death, ShouldDeath));
        _enemyStateMachine.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Death, ShouldDeath));
        _enemyStateMachine.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Death, ShouldDeath));
        
        _enemyStateMachine.SetStartState(EnemyState.Idle);
        _enemyStateMachine.Init();
        
        _followPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerEnter;
        _followPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerExit;

        _rangeAttackPlayerSensor.OnPlayerEnter += RangeAttackPlayerSensor_OnPlayerEnter;
        _rangeAttackPlayerSensor.OnPlayerExit += RangeAttackPlayerSensor_OnPlayerExit;

        _enemyVitality.OnDeath += DisableEnemy;
    }

    private void OnDestroy()
    {
        _followPlayerSensor.OnPlayerEnter -= FollowPlayerSensor_OnPlayerEnter;
        _followPlayerSensor.OnPlayerExit -= FollowPlayerSensor_OnPlayerExit;

        _rangeAttackPlayerSensor.OnPlayerEnter -= RangeAttackPlayerSensor_OnPlayerEnter;
        _rangeAttackPlayerSensor.OnPlayerExit -= RangeAttackPlayerSensor_OnPlayerExit;

        _enemyVitality.OnDeath -= DisableEnemy;
    }

    private bool ShouldDeath(Transition<EnemyState> transition) =>
        _enemyVitality.IsDead;

    private bool IsWithinIdleRange(Transition<EnemyState> transition) =>
        _enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance;

    private bool IsNotWithinIdleRange(Transition<EnemyState> transition) =>
        !IsWithinIdleRange(transition);

    private bool ShouldAttack(Transition<EnemyState> transition) =>
        _lastAttackTime + _attackCooldown <= Time.time && _isInAttackingRange;
    
    private void DisableEnemy(Vector3 position)
    {
        _progress.PlayerData.Progress.WithdrawEnemyCount(1);
        _enemyCollider.enabled = false;
        _enemyVitality.enabled = false;
    }

    private void RangeAttackPlayerSensor_OnPlayerExit(Vector3 lastknownposition) => _isInAttackingRange = false;

    private void RangeAttackPlayerSensor_OnPlayerEnter(Transform player) => _isInAttackingRange = true;

    private void FollowPlayerSensor_OnPlayerExit(Vector3 lastknownposition)
    {
        _enemyStateMachine.Trigger(StateEvent.LosePlayer);
        _isInChasingRange = false;
    }

    private void FollowPlayerSensor_OnPlayerEnter(Transform player)
    {
        _enemyStateMachine.Trigger(StateEvent.DetectPlayer);
        _isInChasingRange = true;
    }

    private void OnAttack(State<EnemyState, StateEvent> State)
    {
        transform.LookAt(_playerVitality.transform.position);
        _lastAttackTime = Time.time;
        _playerVitality.TakeDamage(10);
    }

    private void Update()
    {
        _enemyStateMachine.OnLogic();
    }
}


























