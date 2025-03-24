using System;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private void Awake()
    {
        Managers.Stage.RegisterMonster(gameObject); 
    }

    private void Die()
    {
        Managers.Stage.UnregisterMonster(gameObject);
        Destroy(gameObject); 
    }
}
