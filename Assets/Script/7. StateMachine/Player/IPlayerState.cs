public enum PlayerStateType
{
    Idle,
    Move,
    Attack,
    Death
}

public enum AnimState
{
    Idle,
    Move,
    Attack,
    Death
}

public interface IPlayerState
{
    void Enter();
    void Update();
    void Exit();
} 