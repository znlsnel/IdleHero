using UnityEngine;

public class MoveState : IPlayerState
{
    private readonly PlayerController player;

    public MoveState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Agent.isStopped = false;
        player.SetAnimationState(AnimState.Move);
    }

    public void Update()
    {
        if (player.CurrentTarget == null || !player.CurrentTarget.activeInHierarchy)
        {
            player.ChangeState(PlayerStateType.Idle);
            return;
        }

        player.MoveToTarget();

        float distanceToTarget = Vector3.Distance(player.transform.position, player.CurrentTarget.transform.position);
        if (distanceToTarget <= player.AttackRange)
        {
            player.ChangeState(PlayerStateType.Attack);
        }
    }

    public void Exit() { }
} 