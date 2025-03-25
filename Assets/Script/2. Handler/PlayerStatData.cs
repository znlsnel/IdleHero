using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DesignEnums;

[Serializable]
public class PlayerStatData
{
    private Dictionary<SkillInfo, int> _skills = new Dictionary<SkillInfo, int>();
    private float[] stats;

    // 플레이어 기본 속성
    [SerializeField] public int MaxHealth {get; set;} = 100;
    [SerializeField] public int Health {get; set;} = 100; 
    [SerializeField] public int Experience {get; set;} = 0;
    [SerializeField] public int MaxExperience {get; set;} = 100;
    [SerializeField] public int Coins {get; set;} = 0; 
 
    public event Action OnCheangeValue;

    public PlayerStatData()
    {
        stats = new float[Enum.GetValues(typeof(EStat)).Length];
        stats[(int)EStat.AttackRange] = 1f;  
        stats[(int)EStat.Damage] = 3f; 
        stats[(int)EStat.AttackRate] = 1f;
        stats[(int)EStat.MoveSpeed] = 5f; 
        stats[(int)EStat.ManaRecoveryRate] = 0f;
        stats[(int)EStat.HealthRecoveryRate] = 0f; 
        stats[(int)EStat.EvasionRate] = 0f;
        stats[(int)EStat.Armor] = 0f;
        stats[(int)EStat.CriticalHitRate] = 10f; 
         
        
    }
    
    public void Init()
    {
        Managers.Instance.StartCoroutine(UpdateCondition(1f));
    }

    IEnumerator UpdateCondition(float time)
    {
        while (true)
        {
            AddHealth((int)stats[(int)EStat.HealthRecoveryRate]);
            yield return new WaitForSeconds(time);
        }
    }

    public void AddSkill(SkillInfo skill)
    {
        if (_skills.ContainsKey(skill))
        {
            _skills[skill]++;
        }
        else
        {
            _skills[skill] = 1;
        }

            OnCheangeValue?.Invoke();

        Debug.Log($"AddSkill: {skill.Name} {_skills[skill]}"); 
    }

    public void RemoveSkill(SkillInfo skill)
    {
        if (_skills.ContainsKey(skill))
        {
            _skills[skill]--;
            if (_skills[skill] <= 0)
            {
                _skills.Remove(skill);
            }
        }   
        OnCheangeValue?.Invoke();
    }

    public int GetStat(EStat type)
    {
        float stat = stats[(int)type];

        float rate = 0f;
        // 스킬 효과를 합산
        foreach (var skillEntry in _skills)
        {
            SkillInfo skill = skillEntry.Key;
            int skillCount = skillEntry.Value;

            for (int i = 0; i < skill.Stat.Count; i++)
            {
                if (skill.Stat[i] == type)
                {
                    rate += skill.StatValue[i] * skillCount;
                }
            }
        }

        return (int)(stat * (1 + rate * 0.01f));  
    }

    public void AddHealth(int amount)
    {
        Health += amount;
        if (Health > MaxHealth)
            Health = MaxHealth;
        
        Debug.Log($"Health increased: {Health}");
        OnCheangeValue?.Invoke();
    }

    public void SubtractHealth(int amount)
    {
        Health -= amount;
        if (Health < 0)
            Health = 0;

        Debug.Log($"Health decreased: {Health}"); 
        OnCheangeValue?.Invoke();
    }

    public void IncreaseCoins(int amount) 
    {
        Coins += amount;
        Debug.Log($"Coins increased: {Coins}");
        OnCheangeValue?.Invoke();
    }

    public void DecreaseCoins(int amount)
    {
        Coins -= amount;
        Debug.Log($"Coins decreased: {Coins}"); 
        OnCheangeValue?.Invoke();
    }
}
