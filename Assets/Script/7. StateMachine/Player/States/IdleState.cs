using UnityEngine;

public class IdleState : IPlayerState
{
    private readonly PlayerController player;

    public IdleState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Agent.isStopped = true;
        player.SetAnimationState(AnimState.Idle);
    }

    public void Update()
    {
        if (player.FindNextTarget() != null)
        {
            player.ChangeState(PlayerStateType.Move);
        }
    }

    public void Exit() { }
} 