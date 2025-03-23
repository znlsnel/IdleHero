
using System.Runtime.InteropServices;
using UnityEngine;

public interface IManager
{
    void Init();
    void Clear();
}

public class Managers : Singleton<Managers>
{
    [field: SerializeField] private InputManager input = new InputManager();
    [field: SerializeField] private ResourceManager resource = new ResourceManager();
    [field: SerializeField] private EventManager @event = new EventManager();
    [field: SerializeField] private SoundManager sound = new SoundManager();
    [field: SerializeField] private UIManager ui = new UIManager();
    [field: SerializeField] private PoolManager pool = new PoolManager();

    public static InputManager Input => Instance.input;
    public static ResourceManager Resource => Instance.resource;
    public static EventManager Event => Instance.@event;
    public static SoundManager Sound => Instance.sound;
    public static UIManager UI => Instance.ui;
    public static PoolManager Pool => Instance.pool;
 
    void Start()
    {
        Init();
	}

    void Update()
    {
        //_input.OnUpdate();
    }

    static void Init()
    {
        if (Instance == null)
        {
			GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            // Instance = go.GetComponent<Managers>();

            // Instance.Input.Init();
            // Instance.Resource.Init();
            // Instance.Sound.Init();
        }		
	} 

    public static void Clear()
    {
        // Input.Clear();
        // Sound.Clear();
        // Scene.Clear();
        // UI.Clear();
        // Pool.Clear();
    }
    
}
