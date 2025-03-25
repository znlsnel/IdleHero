using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
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

    public void SetMoveHash(bool isMove)
    {
        animator.SetBool(MoveHash, isMove);
    }


    public void SetDeathHash(bool isDeath)
    {
        animator.SetBool(DeathHash, isDeath);
    }
}
