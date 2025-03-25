using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using static DesignEnums;
using System.Collections;
using System.ComponentModel;

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
    [field: SerializeField] public PlayerStatHandler playerStatHandler { get; private set; } = new PlayerStatHandler();

    // Component
    public PlayerAnimationHandler animationHandler {get; private set;}
    private TargetSensorHandler targetSensorHandler;
    private NavMeshAgent agent;

    // Monster
    private List<GameObject> monsters;
    public float AttackRange => attackRange; 

    // Values
    private float currentHealth;
    private GameObject currentTarget;


    // Properties
    private EPlayerState currentState = EPlayerState.Idle;
    private float attackRange => playerStatHandler.GetStat(EStat.AttackRange);

    void Awake()
    {
        animationHandler = gameObject.GetOrAddComponent<PlayerAnimationHandler>(); 
        targetSensorHandler = GetComponentInChildren<TargetSensorHandler>(); 
        currentHealth = playerStatHandler.MaxHealth;
        agent = gameObject.GetOrAddComponent<NavMeshAgent>();  
        agent.enabled = false;
        Invoke(nameof(Init), 1.0f);  
    } 

    private void Init()
    {
        agent.enabled = true;
    }

    // Stat

    public override void OnDamage(float damage)
    {
        
    }
  
    public override void OnAttack()
    {
        foreach (var target in targetSensorHandler.OverlabTargets)
        { 
            target.GetComponent<BattleObject>()?.OnDamage(playerStatHandler.GetStat(EStat.Damage));
        }
    }

    private void Update()
    {
        if (agent.enabled && agent.isOnNavMesh)
            UpdateState();
    } 
    public void SetState(EPlayerState state)
    {
        currentState = state;
        agent.isStopped = state == EPlayerState.Idle;
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
        bool isAttackable = IsAttackable();
        animationHandler.SetAttackHash(isAttackable); 

        if (!isAttackable) 
            SetState(EPlayerState.Idle);
         
    }

    public void OnDeathState()
    {       
        animationHandler.SetDeathHash(true);
    }
 
    private bool IsAttackable()
    {
        return Vector3.Distance(currentTarget.transform.position, transform.position) <= attackRange;
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
}


