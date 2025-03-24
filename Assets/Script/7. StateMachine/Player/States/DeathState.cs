using UnityEngine;

public class DeathState : IPlayerState
{
    private readonly PlayerController player;

    public DeathState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Agent.isStopped = true;
        player.SetAnimationState(AnimState.Death);
    }

    public void Update() { }

    public void Exit() { }
} 