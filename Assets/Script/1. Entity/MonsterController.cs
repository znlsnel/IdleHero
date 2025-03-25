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


public class MonsterController : BattleObject, IPoolable
{
    // Animation Data
    private readonly static int moveHash = Animator.StringToHash("Move");
    private readonly static int attackHash = Animator.StringToHash("Attack");
    private readonly static int deathHash = Animator.StringToHash("Death"); 


    // Datas
    [SerializeField] private MonsterSO monsterSO;

    // Components
    private TargetSensorHandler targetSensorHandler;
    private Rigidbody rigidbody;
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
    public bool IsDead => isDead; 

    // Values
    private MonsterState currentState = MonsterState.Idle;
    private float lastAttackTime;
    private float currentHealth;
    private bool isDead = false;

    // Event
    public Action<GameObject> onDie;

    private void Awake()
    {
        agent = gameObject.GetOrAddComponent<NavMeshAgent>(); 
        animator = GetComponentInChildren<Animator>();
        targetSensorHandler = GetComponentInChildren<TargetSensorHandler>();
        rigidbody = GetComponent<Rigidbody>();
    } 

    private void Update()
    {
        if (agent.enabled)
            UpdateState(); 
          
    }
    public void Init() 
    {
        LocateNavMeshPosition();
        SetState(MonsterState.Idle); 
        
        currentHealth = monsterSO.MaxHealth;
        lastAttackTime = Time.time;
        isDead = false;
        agent.speed = MoveSpeed;  
    }

    private void LocateNavMeshPosition()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent.enabled = true; 
        }
    }

    #region  IPoolable

    public void Initialize(Action<GameObject> returnAction)
    {
        onDie = returnAction;
        Invoke(nameof(Init), 0.1f);
    }

    public void OnSpawn()
    {
        
    }

    public void OnDespawn()
    {
        animator.SetBool(deathHash, false);

        onDie?.Invoke(gameObject);   
    } 
    
    #endregion
    #region Animation Event
    public override void OnDamage(float damage)
    {
        if (currentState == MonsterState.Death)
            return;
 
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            SetState(MonsterState.Death);
        } 

        DamageUI damageUI = Managers.UI.ShowSceneChildUI<DamageUI>();
        damageUI.InitText(damage.ToString(), gameObject);  

        Destroy(damageUI.gameObject, 1.5f); 
    }
    public override void OnAttack()
    {
        foreach (var target in targetSensorHandler.OverlabTargets)
        {
            if (target.TryGetComponent(out PlayerController playerController))
            {
                playerController.OnDamage(AttackDamage); 
            }
        }
    }

    #endregion
    #region State
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
        agent.isStopped = currentState != MonsterState.Trace;  
        if (agent.isStopped )
            rigidbody.velocity *= 0.1f;
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
        rigidbody.velocity *= 0.1f;

        transform.LookAt(player.transform);

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

        Invoke(nameof(OnDespawn), 1f); 
    }
    #endregion

    private bool IsTracable(float distance)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= TraceRange;
    }

    private bool IsAttackable(float distance)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= AttackRange;
    }


}
 