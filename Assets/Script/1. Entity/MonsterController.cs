using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState
{
    Idle,
    Trace, 
    Attack,
    Death
}

public abstract class BattleObject : MonoBehaviour
{
    public abstract void OnDamage(float damage);
    public abstract void OnAttack(); 
}


[RequireComponent(typeof(NavMeshAgent))]
public class MonsterController : BattleObject, IPoolable
{
    // Animation Data
    private readonly static int moveHash = Animator.StringToHash("Move");
    private readonly static int attackHash = Animator.StringToHash("Attack");
    private readonly static int deathHash = Animator.StringToHash("Death"); 


    // Datas
    [SerializeField] private MonsterSO monsterSO;

    // Components
    private NavMeshAgent agent;
    private Animator animator;

    // Properties
    private GameObject player => Managers.Player;
    private float MoveSpeed => monsterSO.MoveSpeed;
    private float AttackRange => monsterSO.AttackRange;
    private float TraceRange => monsterSO.TraceRange;
    private float AttackDamage => monsterSO.Damage;
    private float AttackDelay => monsterSO.AttackDelay; 
    private float DistanceToPlayer => Vector3.Distance(transform.position, player.transform.position);

    // Values
    private MonsterState currentState = MonsterState.Idle;
    private float lastAttackTime;
    private float currentHealth;
    private bool isDead = false;
    // Event
    public Action<GameObject> onDie;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        agent.speed = MoveSpeed; 
    }

    #region  IPoolable
    private void Update()
    {
        UpdateState();
    }

    public void Initialize(Action<GameObject> returnAction)
    {
        onDie = returnAction;
        OnSpawn();
    }
 
    public void OnSpawn()
    {
        SetState(MonsterState.Idle); 
        currentHealth = monsterSO.MaxHealth;
        lastAttackTime = Time.time;
        Managers.Stage.RegisterMonster(gameObject); 
        isDead = false;
    }

    public void OnDespawn()
    {
        onDie?.Invoke(gameObject); 
        animator.SetBool(deathHash, false);
        Managers.Stage.UnregisterMonster(gameObject);  
    }
    #endregion
    public override void OnDamage(float damage)
    {
        if (currentState == MonsterState.Death)
            return;
 
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            SetState(MonsterState.Death);
        } 
    }
    public override void OnAttack()
    {
        
    }
 
    
    private void UpdateState()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                OnIdleState();
                break;
            case MonsterState.Trace:
                OnTraceState();
                break;
            case MonsterState.Attack:
                OnAttackState();
                break;
            case MonsterState.Death:
                OnDeathState();
                break;
        }
    }
    private void SetState(MonsterState newState)
    {
        currentState = newState;
        agent.isStopped = currentState == MonsterState.Trace;
        
    }

    
    private void OnIdleState()
    {
        // 추적이 가능하다면 추적 상태로 
        animator.SetBool(moveHash, false);
 
        if (IsTracable(DistanceToPlayer))
        {
            SetState(MonsterState.Trace);
            return;
        } 
    }

    private void OnTraceState()
    {
        agent.SetDestination(player.transform.position);

        bool isAttackable = IsAttackable(DistanceToPlayer);
        bool isTracable = IsTracable(DistanceToPlayer);

        animator.SetBool(moveHash, !isTracable && isTracable); 
        // 공격이 가능하다면? 공격상태로 변환
        if (isAttackable)
            SetState(MonsterState.Attack);

        // 추적이 불가능하다면? 아이들 상태로 변환
        else if (!isTracable)
            SetState(MonsterState.Idle);
    }

    private void OnAttackState()
    { 
        agent.isStopped = false;

        // 공격할 수 없다면 추적 모드로 변환
        if (!IsAttackable(DistanceToPlayer))
        {
            SetState(MonsterState.Trace);
            return;
        }

        if (Time.time >= lastAttackTime + AttackDelay)
        {
            animator.SetTrigger(attackHash);
            lastAttackTime = Time.time;
        } 
    }
     
    private void OnDeathState()
    {
        if (isDead)
            return;

        isDead = true;  
        agent.isStopped = true;
        animator.SetBool(deathHash, true);

        Invoke(nameof(OnDespawn), 3f); 
    }

    private bool IsTracable(float distance)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= TraceRange;
    }

    private bool IsAttackable(float distance)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= AttackRange;
    }

}
