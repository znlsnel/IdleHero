using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using static DesignEnums;
using System.Collections;
using System.ComponentModel;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Animations;

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
    [SerializeField] private GameObject _attackParticle;
    [SerializeField] private GameObject _criticalParticle;
    // Component
    public PlayerAnimationHandler animationHandler {get; private set;}
    private TargetSensorHandler targetSensorHandler;
    private NavMeshAgent agent;
    private Rigidbody rigidbody;

    // Monster
    private List<GameObject> monsters;
    private GameObject currentTarget; 


    // Properties
    private EPlayerState currentState = EPlayerState.Idle;
    private float attackRange => playerStatData.GetStat(EStat.AttackRange);
    private float footStepTime = 0.3f;  
    private float lastFootStepTime = 0f;  

    // Event Action
    public event Action OnPlayerAttack;
    public event Action OnPlayerDie;

    private void Awake()
    {
        targetSensorHandler = GetComponentInChildren<TargetSensorHandler>(); 
        animationHandler = gameObject.GetOrAddComponent<PlayerAnimationHandler>(); 
        agent = gameObject.GetOrAddComponent<NavMeshAgent>();   

        rigidbody = GetComponent<Rigidbody>();
 
        playerStatData.Init(); 
        
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
        PlayFootStepSound();
    } 
     
    private void PlayFootStepSound()
    {
        if (Time.time - lastFootStepTime < footStepTime || currentState != EPlayerState.Move)
            return;

        lastFootStepTime = Time.time;
        Managers.Sound.Play($"FootStep/SFX_Movement_Footstep_Water_{Random.Range(1, 5)}", 0.5f);
    }  

    public void Init()
    { 
        //agent.Warp(new Vector3(0, 0, 4)); 
        agent.enabled = true; 
        playerStatData.Health = playerStatData.MaxHealth;
        agent.speed = playerStatData.GetStat(EStat.MoveSpeed);
        animationHandler.SetDeathHash(false);  
        animationHandler.SetAttackHash(false);    

        SetState(EPlayerState.Idle); 
    }

 
    public override void OnDamage(float damage, GameObject particle) 
    { 
        if (currentState == EPlayerState.Death)
            return;

        if (Random.Range(0, 100) < playerStatData.GetStat(EStat.EvasionRate))
            return;

        float armorDamage = damage * (playerStatData.GetStat(EStat.Armor) * 0.01f);
        damage = Mathf.Clamp(damage, damage / 2, damage - armorDamage); 
        playerStatData.SubtractHealth((int)damage);  

        if (playerStatData.Health <= 0)
        {
            currentState = EPlayerState.Death;
            SetState(EPlayerState.Death);
            Managers.Sound.Play("UI/SFX_UI_Bonus_1", 1f); 
            OnPlayerDie?.Invoke();
            Managers.SetTimer(()=>
            {
                Init(); 
                Managers.Stage.Restart(); 
                
            }, 2.0f); 
        }

        var go = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(go, 2.5f);
    }
  
    public override void OnAttack()
    {
        bool flag = false;
        foreach (var target in targetSensorHandler.OverlabTargets)
        { 
            if (target.TryGetComponent(out MonsterController monsterController) == false)
                continue;

            if (monsterController.IsDead)
                continue; 

            flag = true;
            float damage = playerStatData.GetStat(EStat.Damage);
            bool isCritical = Random.Range(0, 100) < playerStatData.GetStat(EStat.CriticalHitRate);
            if (isCritical)
                damage *= 2;
 
            target.GetComponent<BattleObject>()?.OnDamage(damage, isCritical ? _criticalParticle : _attackParticle);
        }
        OnPlayerAttack?.Invoke();

        if (flag)
            Managers.Sound.Play($"Attack/SFX_Confetti_Explosion_{Random.Range(1, 3)}", 1f);    
        else
            Managers.Sound.Play($"Attack/SFX_Movement_Swoosh_Fast_{Random.Range(1, 4)}", 0.2f);   
 
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
        if (monsters == null || monsters.Count == 0 || currentTarget.GetComponent<MonsterController>().IsDead)
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
        bool monster = monsters != null && monsters.Count > 0  && currentTarget.GetComponent<MonsterController>().IsDead == false;
        return monster && Vector3.Distance(currentTarget.transform.position, transform.position) <= attackRange; 
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

 
            yield return new WaitForSeconds(0.1f); 
        }
    }
    
    #endregion

}


