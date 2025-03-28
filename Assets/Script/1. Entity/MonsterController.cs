using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public enum MonsterState
{
    Idle,
    Trace, 
    Attack,
    Death
}

public abstract class BattleObject : MonoBehaviour
{
    public abstract void OnDamage(long damage, GameObject particle); 
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
    [SerializeField] private GameObject _attackParticle;
    // Components
    private TargetSensorHandler targetSensorHandler;
    private NavMeshAgent agent;
    private Animator animator;

    // Properties
    private GameObject player => Managers.Player;
    private float MoveSpeed => monsterSO.MoveSpeed;
    private float AttackRange => monsterSO.AttackRange;
    private float TraceRange => monsterSO.TraceRange;
    private long AttackDamage => monsterSO.Damage;
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
    } 

    private void Update()
    {
        if (agent.enabled)
            UpdateState(); 
          
    }
    private void Init() 
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
    #region Battle Function
    public override void OnDamage(long damage, GameObject particle)
    {
        if (currentState == MonsterState.Death)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            SetState(MonsterState.Death);
            Managers.Player.GetComponent<PlayerController>().playerStatData.Coins += 100; 
        } 
 
        DamageUI damageUI = Managers.UI.ShowSceneChildUI<DamageUI>();
        damageUI.InitText( Util.ConvertBigint(damage), gameObject);  
        var go = Instantiate(particle, transform.position + Vector3.up * 1.5f, Quaternion.identity);  
        Destroy(go, 2.5f);

       
        Destroy(damageUI.gameObject, 1.5f); 
    }
    public override void OnAttack()
    { 
        foreach (var target in targetSensorHandler.OverlabTargets)
        {
            if (target.TryGetComponent(out PlayerController playerController))
            {
                playerController.OnDamage(AttackDamage, _attackParticle); 
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
 