using UnityEngine;

public class AttackState : IPlayerState
{
    private readonly PlayerController player;
    private float attackDuration = 0.5f;
    private float elapsedTime = 0f;
    private int comboCount = 0;
    private int MaxComboCnt = 3; 

    public AttackState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Agent.isStopped = true; 
        player.SetAnimationState(AnimState.Attack);
        player.animationHandler.SetComboHash(comboCount); 

        comboCount = (comboCount + 1) % MaxComboCnt; 
        
        elapsedTime = 0f;
    }

    public void Update()
    {
        if (player.CurrentTarget == null)
        {
            player.ChangeState(PlayerStateType.Idle);
            return;
        }

        player.RotateTowardsTarget(720f);
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= attackDuration)
        {
            // Apply damage logic here
            player.ChangeState(PlayerStateType.Move);
        }
    }

    public void Exit() { }
} 