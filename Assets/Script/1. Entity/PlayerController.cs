using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using static DesignEnums;
using System.Collections;
using System.ComponentModel;
using System;
using Random = UnityEngine.Random;

public enum EPlayerState
{
    Idle,
    Move,
    Attack,
    Death
}

 

[RequireComponent(typeof(PlayerAnimationHandler))]
public class PlayerController : BattleObject
{
    [field: SerializeField] public PlayerStatData playerStatData { get; private set; } = new PlayerStatData();

    // Component
    public PlayerAnimationHandler animationHandler {get; private set;}
    private TargetSensorHandler targetSensorHandler;
    private NavMeshAgent agent;
    private Rigidbody rigidbody;

    // Monster
    private List<GameObject> monsters;
    public float AttackRange => attackRange; 

    private GameObject currentTarget; 

    // Properties
    private EPlayerState currentState = EPlayerState.Idle;
    private float attackRange => playerStatData.GetStat(EStat.AttackRange);

    // Event Action
    public event Action OnPlayerAttack;

    private void Awake()
    {
        targetSensorHandler = GetComponentInChildren<TargetSensorHandler>(); 
        
        animationHandler = gameObject.GetOrAddComponent<PlayerAnimationHandler>(); 
        agent = gameObject.GetOrAddComponent<NavMeshAgent>();   

        rigidbody = GetComponent<Rigidbody>();
 
        playerStatData.Init(); 
        playerStatData.Health = playerStatData.MaxHealth;
        agent.enabled = false;

        Invoke(nameof(Init), 1.0f);  
    }
    private void Start()
    {
        Managers.Stage.OnStageClear += () =>
        {
            targetSensorHandler.Clear();
        };
    }
    private void Update()
    {
        if (agent.enabled && agent.isOnNavMesh)
            UpdateState();

        agent.speed = playerStatData.GetStat(EStat.MoveSpeed); 
        targetSensorHandler.transform.localScale = new Vector3(attackRange, attackRange, attackRange);
    } 
    
    private void Init()
    {
        agent.enabled = true;
        agent.speed = playerStatData.GetStat(EStat.MoveSpeed);
    }
 
    public override void OnDamage(float damage)
    { 
        if (Random.Range(0, 100) < playerStatData.GetStat(EStat.EvasionRate))
            return;

        float armorDamage = damage * (playerStatData.GetStat(EStat.Armor) * 0.01f);
        damage = Mathf.Clamp(damage, damage / 2, damage - armorDamage); 
        playerStatData.SubtractHealth((int)damage);  
    }
  
    public override void OnAttack()
    {
        foreach (var target in targetSensorHandler.OverlabTargets)
        { 
            if (target.TryGetComponent(out MonsterController monsterController) == false)
                continue;

            if (monsterController.IsDead)
                continue; 


            int damage = playerStatData.GetStat(EStat.Damage);
            if (Random.Range(0, 100) < playerStatData.GetStat(EStat.CriticalHitRate))
                damage *= 2;


            target.GetComponent<BattleObject>()?.OnDamage(damage);
        }
        OnPlayerAttack?.Invoke();
    }

 

    #region State Pattern
    public void SetState(EPlayerState state)
    {
        currentState = state;
        agent.isStopped = state != EPlayerState.Move; 
        animationHandler.SetSpeed(state == EPlayerState.Attack ? 
            playerStatData.GetStat(EStat.AttackRate) : 1.0f);  
    }

    public void UpdateState()
    {
        switch(currentState)
        {
            case EPlayerState.Idle:
                OnIdleState();
                break;
            case EPlayerState.Move:
                OnMoveState();
                break;
            case EPlayerState.Attack:
                OnAttackState();
                break;
            case EPlayerState.Death:
                OnDeathState();
                break;
        }
    }


    public void OnIdleState()
    {
        if (monsters == null || monsters.Count == 0)
            return;
        
        SetState(EPlayerState.Move);
        animationHandler.SetMoveHash(false);
    }

    public void OnMoveState()
    {
        animationHandler.SetMoveHash(true); 

        if (currentTarget != null)
            agent.SetDestination(currentTarget.transform.position);

        if (IsAttackable()) 
        {
            SetState(EPlayerState.Attack); 
            animationHandler.SetMoveHash(false);  
        }
        
        else if (currentTarget == null)
            SetState(EPlayerState.Idle);
    }

    public void OnAttackState()
    {
        if (currentTarget != null) 
            transform.LookAt(currentTarget.transform);

        

        bool isAttackable = IsAttackable();
        animationHandler.SetAttackHash(isAttackable); 

        if (!isAttackable) 
            SetState(EPlayerState.Idle);

        rigidbody.velocity *= 0.1f;
    }

    public void OnDeathState()
    {       
        animationHandler.SetDeathHash(true);
    }
    #endregion
    
    #region Utility
    private bool IsAttackable()
    {

        return monsters != null && monsters.Count > 0 && Vector3.Distance(currentTarget.transform.position, transform.position) <= attackRange;
    }
    public void SetStageMonster(List<GameObject> monsters)
    {
        this.monsters = monsters; 
        this.monsters.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position)));
        currentTarget = monsters[0];

        StartCoroutine(UpdateTargetMonster());
    }
    private IEnumerator UpdateTargetMonster()
    {
        while(true)
        {   
            if (monsters != null && monsters.Count > 0)
            {
                monsters.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position)));
                currentTarget = monsters[0];
            }

            
            

            yield return new WaitForSeconds(0.3f); 
        }
    }
    
    #endregion
    
    private IEnumerator SetTimer(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();

    }

}


