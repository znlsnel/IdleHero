using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using static DesignEnums;
using System.Collections;
using System.ComponentModel;




[RequireComponent(typeof(PlayerAnimationHandler))]
public class PlayerController : BattleObject
{
    [field: SerializeField] public PlayerStatHandler playerStatHandler { get; private set; } = new PlayerStatHandler();

    // Component
    public PlayerAnimationHandler animationHandler {get; private set;}
    private NavMeshAgent agent;

    // Monster
    private List<GameObject> monsters;
    public GameObject CurrentTarget {get; private set;}
    public NavMeshAgent Agent => agent;
    public float AttackRange => attackRange; 
    
    private float currentHealth;

    // Stat
    private float attackRange => playerStatHandler.GetStat(EStat.AttackRange);

    // State
    private IPlayerState currentState;
    private Dictionary<PlayerStateType, IPlayerState> states;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animationHandler = gameObject.GetOrAddComponent<PlayerAnimationHandler>();
        currentHealth = playerStatHandler.MaxHealth;
        
        FindMonsters();
        InitializeStates();
        ChangeState(PlayerStateType.Idle);
    }

    private void InitializeStates()
    {
        states = new Dictionary<PlayerStateType, IPlayerState> 
        {
            { PlayerStateType.Idle, new IdleState(this) },
            { PlayerStateType.Move, new MoveState(this) },
            { PlayerStateType.Attack, new AttackState(this) },
            { PlayerStateType.Death, new DeathState(this) }
        };
    }

    public void ChangeState(PlayerStateType newStateType)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = states[newStateType];
        currentState.Enter();
    }

    private void Update()
    {
        if (currentState != null)
            currentState.Update();
    }

    public override void OnDamage(float damage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            OnDeath();
        } 
    }
 
    public override void OnAttack()
    {
        CurrentTarget.GetComponent<BattleObject>().OnDamage(playerStatHandler.GetStat(EStat.Damage)); 
    } 
    public void FindMonsters()
    {
        monsters = Managers.Stage.GetMonsters().ToList();
    }

    public GameObject FindNextTarget()
    {
        CurrentTarget = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject monster in monsters)
        {
            if (monster.activeInHierarchy)
            {
                float distance = Vector3.Distance(transform.position, monster.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    CurrentTarget = monster;
                }
            }
        }

        return CurrentTarget;
    }

    public void MoveToTarget()
    {
        if (CurrentTarget != null)
        {
            agent.SetDestination(CurrentTarget.transform.position);
            SetAnimationState(AnimState.Move);
        }
    }

    public void RotateTowardsTarget(float rotationSpeed)
    {
        if (CurrentTarget == null)
            return;
            
        Vector3 directionToTarget = (CurrentTarget.transform.position - transform.position).normalized;
        directionToTarget.y = 0;
        
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationAmount);
        }
    }

    public void SetAnimationState(AnimState state)
    {
        animationHandler.SetIdleHash(false);
        animationHandler.SetMoveHash(false);
        animationHandler.SetAttackHash(false);
        
        switch (state)
        {
            case AnimState.Idle:
                animationHandler.SetIdleHash(true);
                break;
            case AnimState.Move:
                animationHandler.SetMoveHash(true);
                break;
            case AnimState.Attack:
                animationHandler.SetAttackHash(true);
                break;
            case AnimState.Death:
                animationHandler.SetDeathHash(true);
                break;
        }
    }

    public void OnDeath()
    {
        ChangeState(PlayerStateType.Death);
    }


}


