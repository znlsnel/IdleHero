using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int ComboHash = Animator.StringToHash("Combo");
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int MoveHash = Animator.StringToHash("Move");
    private static readonly int DeathHash = Animator.StringToHash("Death");
    
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetAttackHash(bool isAttack)
    {
        animator.SetBool(AttackHash, isAttack);
    }

    public void SetIdleHash(bool isIdle)
    {
        animator.SetBool(IdleHash, isIdle);
    }

    public void SetMoveHash(bool isMove)
    {
        animator.SetBool(MoveHash, isMove);
    }

    public void SetComboHash(int comboCount)
    {
        animator.SetInteger(ComboHash, comboCount);
    } 

    public void SetDeathHash(bool isDeath)
    {
        animator.SetBool(DeathHash, isDeath);
    }
}
