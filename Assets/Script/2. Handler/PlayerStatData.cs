using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static DesignEnums;

[Serializable]
public class PlayerStatData
{
    [field: SerializeField] private StatDataSO _statDataSO;

    private Dictionary<SkillInfo, int> _skills = new Dictionary<SkillInfo, int>();
    private Dictionary<EStat, long> _statDataDict = new Dictionary<EStat, long>();
    private Dictionary<EStat, int> _statLevelDict = new Dictionary<EStat, int>();
    
    private long[] _statPrice = new long[1001];
    public long[] SkillStats {get; private set;}
    public long Health {get; private set;} 
 
    public int GetLevel(EStat statType) => _statLevelDict[statType];
    public void SetLevel(EStat statType, int level) => _statLevelDict[statType] = level;
    public long GetLevelPrice(int level) => _statPrice[level];
 

    // 플레이어 기본 속성
    [SerializeField] public long Coins {get; set;} = 0;  
 
    // 플레이어
    public event Action OnCheangeValue;
  
    public PlayerStatData()
    {  
 
    } 
      
    public void Init()
    {
        SkillStats = new long[Enum.GetValues(typeof(EStat)).Length];

        foreach (var statData in _statDataSO.itemDatas)
        {
            _statDataDict[statData.statType] = statData.value;
            _statLevelDict[statData.statType] = 0;
        }


        foreach (EStat statType in Enum.GetValues(typeof(EStat)))
        {
            if (!_statDataDict.ContainsKey(statType))
            {
                _statDataDict[statType] = 0;
                _statLevelDict[statType] = 0;  
            }
        }
 
        _statPrice[0] = _statDataSO.startPrice;
        for (int i = 1; i < _statPrice.Length; i++)
            _statPrice[i] = _statPrice[i - 1] + (long)(_statPrice[i - 1] * (_statDataSO.priceIncreaseRate * 0.01f));   
        

        Managers.Instance.StartCoroutine(UpdateCondition(1f));
    }
 
    IEnumerator UpdateCondition(float time)
    {
        while (true)
        {
            if (Health > 0)
                Heal(SkillStats[(int)EStat.HealthRecoveryRate] / 10);     
            
            
            yield return new WaitForSeconds(time);
        }
    } 
    public void AddSkill(SkillInfo skill)
    {
        _skills[skill] = _skills.ContainsKey(skill) ? _skills[skill] + 1 : 1;
        
        for (int i = 0; i < skill.Stat.Count; i++)
            SkillStats[(int)skill.Stat[i]] += skill.StatValue[i];

        OnCheangeValue?.Invoke();
    }

    public long GetStat(EStat type)
    {
        long stat = _statDataDict[type];
        float rate = SkillStats[(int)type] * 0.01f;

        return (long)(stat * (1 + rate));     
    }
 
 
	public void ModifyStat(EStat statType, long amount, bool set = false)
	{
		if (!_statDataDict.ContainsKey(statType)) return;
		_statDataDict[statType] = set ? amount : _statDataDict[statType] + amount; 

        OnCheangeValue?.Invoke();
	}
 
    public void Heal(long value)
    { 
        Health = (long)Mathf.Clamp(Health + value, 0, _statDataDict[EStat.MaxHealth]);
        OnCheangeValue?.Invoke();
    }
 
    public void Damage(int value)
    {
        Health = (long)Mathf.Clamp(Health - value, 0, _statDataDict[EStat.MaxHealth]);
        OnCheangeValue?.Invoke(); 
    }
}
