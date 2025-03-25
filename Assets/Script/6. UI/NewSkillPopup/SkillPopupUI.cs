using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;



public class SkillPopupUI : UI_Popup
{
    enum GameObjects
    {
        SkillParent
    }

    private List<SkillPopupButtonUI> _skillButtons = new List<SkillPopupButtonUI>();
    private List<SkillInfo> _selectedSkills = new List<SkillInfo>();
    private PlayerStatHandler _playerStatHandler;

    void Awake()
    {
        _playerStatHandler = Object.FindFirstObjectByType<PlayerController>()?.playerStatHandler;
        Init();
    }

    private void Start()
    {
        LoadRandomSkills();
    } 
 
    public override void Init() 
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));

        Transform parent = GetObject((int)GameObjects.SkillParent).transform;

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject skill = parent.GetChild(i).gameObject;
            SkillPopupButtonUI skillPopupButtonUI = skill.GetOrAddComponent<SkillPopupButtonUI>();
            skillPopupButtonUI.Init();

            _skillButtons.Add(skillPopupButtonUI);

            Button button = skill.GetComponent<Button>();

            int idx = i;
            button.onClick.AddListener(() => { OnClickSkillButton(idx); }); 
        } 
    } 
    private void OnClickSkillButton(int index)
    {
        if (_skillButtons[index].IsSelected)
        {
            DecideSkill(index);
            return;
        }
        
        for (int i = 0; i < _skillButtons.Count; i++)
            _skillButtons[i].SetSelected(i == index); 
         
    } 
    private void DecideSkill(int index)
    {
        _playerStatHandler.AddSkill(_selectedSkills[index]);
        foreach (string skillName in _selectedSkills[index].EffectPrefab)
        {
            GameObject skill =Managers.Resource.Load<GameObject>($"Skill/{skillName}");
            if (skill != null)
                Instantiate(skill); 
        } 
        
        
        ClosePopupUI();
    }
    private void LoadRandomSkills()
    {
        // SkillManager에서 랜덤한 스킬 3개 가져오기
        _selectedSkills.Clear();
        _selectedSkills = Managers.Skill.GetRandomSkills(3); 

        for (int i = 0; i < _selectedSkills.Count && i < _skillButtons.Count; i++)
        {
            _skillButtons[i].SetInfo(_selectedSkills[i]);
        }
    }
} 