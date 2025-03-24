using System.Collections.Generic;
using UnityEngine;

public class SkillManager : IManager
{
    private List<SkillInfo> _skills;

    // 스킬 목록 초기화
    public void Init()
    {
        _skills = DataManager.skillInfoLoader.ItemsList;
    }

    // 랜덤하게 스킬 추출
    public SkillInfo GetRandomSkill()
    {
        if (_skills == null || _skills.Count == 0)
        {
            Debug.LogWarning("스킬 목록이 비어 있습니다.");
            return null;
        }

        int randomIndex = Random.Range(0, _skills.Count);
        return _skills[randomIndex]; 
    }

    // 지정된 개수만큼 랜덤하게 스킬 추출 (중복 최소화)
    public List<SkillInfo> GetRandomSkills(int count)
    {
        if (_skills == null || _skills.Count == 0)
        {
            Debug.LogWarning("스킬 목록이 비어 있습니다.");
            return new List<SkillInfo>();
        }

        if (count <= 0)
        {
            Debug.LogWarning("요청한 스킬 개수가 유효하지 않습니다.");
            return new List<SkillInfo>();
        }

        List<SkillInfo> selectedSkills = new List<SkillInfo>();
        HashSet<int> selectedIndices = new HashSet<int>();

        while (selectedSkills.Count < count && selectedSkills.Count < _skills.Count)
        {
            int randomIndex = Random.Range(0, _skills.Count);

            if (!selectedIndices.Contains(randomIndex))
            {
                selectedSkills.Add(_skills[randomIndex]);
                selectedIndices.Add(randomIndex);
            } 
        }

        return selectedSkills;
    }

    public void Clear()
    {
        
    }
}

