using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static DesignEnums;

[Serializable]
public class StatData
{
    public Sprite icon;
    public EStat statType;
    public string statName; 
    public long value;
    public bool upgradeable;
    public float upgradeRate;
    public bool plusStat;
} 

[CreateAssetMenu(fileName = "StatDataSO", menuName = "ScriptableObject/StatDataSO")]
public class StatDataSO : ScriptableObject 
{
    [SerializeField] public float priceIncreaseRate; 
    [SerializeField] public int startPrice; 



    [field: SerializeField] public List<StatData> itemDatas {get; set;} = new List<StatData>(); 
} 
   