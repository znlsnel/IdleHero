using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[CreateAssetMenu(fileName = "MonsterSO", menuName = "ScriptableObject/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    [field : SerializeField] public string MonsterName {get; private set;}
    [field : SerializeField] public int MaxHealth {get; private set;}
    [field : SerializeField] public int CurrentHealth {get; private set;}
    [field : SerializeField] public int Attack {get; private set;}
    [field : SerializeField] public int AttackDelay {get; private set;}
    [field : SerializeField] public int Experience {get; private set;}
}
 