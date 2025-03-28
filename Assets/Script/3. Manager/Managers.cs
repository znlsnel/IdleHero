
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public interface IManager
{
    void Init();
    void Clear();
}

public class Managers : Singleton<Managers>
{ 
    [field: SerializeField] private DataManager data;
    [field: SerializeField] private ResourceManager resource = new ResourceManager();
    [field: SerializeField] private SoundManager sound = new SoundManager();
    [field: SerializeField] private UIManager ui = new UIManager();
    [field: SerializeField] private PoolManager pool = new PoolManager();
    [field: SerializeField] private StageManager stage = new StageManager();
    [field: SerializeField] private SkillManager skill = new SkillManager(); 
      
    public static DataManager Data => Instance.data;
    public static ResourceManager Resource => Instance.resource;
    public static SoundManager Sound => Instance.sound; 
    public static UIManager UI => Instance.ui;
    public static PoolManager Pool => Instance.pool; 
    public static StageManager Stage => Instance.stage;
    public static SkillManager Skill => Instance.skill;

    public static GameObject Player {get; private set;}
    protected override void Awake()
    {
        base.Awake();

        Player = FindFirstObjectByType<PlayerController>().gameObject;
        data = new DataManager();
        Init();
        
    }

    private static void Init()
    {
        Resource.Init();
        Sound.Init();
        UI.Init();
        Pool.Init();
        Stage.Init();
        Skill.Init();
        
	}
 
    public static void Clear()
    {
        Resource.Clear();
        Sound.Clear();
        UI.Clear();
        Pool.Clear(); 
        Stage.Clear();
        Skill.Clear();
    }
}
