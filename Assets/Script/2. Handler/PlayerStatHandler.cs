using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DesignEnums;

[Serializable]
public class PlayerStatHandler
{
    private Dictionary<SkillInfo, int> _skills = new Dictionary<SkillInfo, int>();
    private float[] stats;

    // 플레이어 기본 속성
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;
    [SerializeField] private int _experience;
    [SerializeField] private int _maxExperience;
    [SerializeField] private int _coins; 

    public event Action OnCheangeValue;

    public PlayerStatHandler()
    {
        stats = new float[Enum.GetValues(typeof(EStat)).Length];
        stats[(int)EStat.AttackRange] = 2f;
        stats[(int)EStat.Damage] = 10f;
        stats[(int)EStat.AttackRate] = 1f;
        stats[(int)EStat.MoveSpeed] = 3.5f;
        stats[(int)EStat.ManaRecoveryRate] = 1f;
        stats[(int)EStat.HealthRecoveryRate] = 1f;
        stats[(int)EStat.EvasionRate] = 1f;
        stats[(int)EStat.Armor] = 1f;
        stats[(int)EStat.CriticalHitRate] = 1f; 
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

    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = Mathf.Max(0, value); // 최대 체력은 0 이상으로 유지
    }

    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            OnCheangeValue?.Invoke();
        }
    }

    public int Coins
    {
        get => _coins;
        set
        {
            _coins = Mathf.Max(0, value);
            OnCheangeValue?.Invoke();
        }
    }

    public int MaxExperience
    {
        get => _maxExperience;
        set => _maxExperience = Mathf.Max(0, value); // 최대 경험치는 0 이상으로 유지
    }

    public int Experience
    {
        get => _experience;
        set
        {
            _experience = Mathf.Clamp(value, 0, MaxExperience);
            OnCheangeValue?.Invoke();
        }
    }

    public void IncreaseHealth(int amount)
    {
        Health += amount;
        Debug.Log($"Health increased: {Health}");
    }

    public void DecreaseHealth(int amount)
    {
        Health -= amount;
        Debug.Log($"Health decreased: {Health}");
    }

    public void IncreaseCoins(int amount)
    {
        Coins += amount;
        Debug.Log($"Coins increased: {Coins}");
    }

    public void DecreaseCoins(int amount)
    {
        Coins -= amount;
        Debug.Log($"Coins decreased: {Coins}");
    }
}
