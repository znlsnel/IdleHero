using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[CreateAssetMenu(fileName = "MonsterSO", menuName = "ScriptableObject/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    [field : SerializeField] public string MonsterName {get; private set;}
    [field : SerializeField] public int Experience {get; private set;}  
    [field : SerializeField] public int MaxHealth {get; private set;}
    [field : SerializeField] public int Damage {get; private set;} 
    [field : SerializeField] public int AttackDelay {get; private set;}
    [field : SerializeField] public int AttackRange {get; private set;}   
    [field : SerializeField] public int TraceRange {get; private set;}   
    [field : SerializeField] public int MoveSpeed {get; private set;}    
} 
 