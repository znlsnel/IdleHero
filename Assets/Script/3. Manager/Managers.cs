
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
 
    private void Start()
    {
        Init();
	}

    private void Update()
    {
        //_input.OnUpdate();
    }

    private static void Init()
    {

        // Instance.Input.Init();
        // Instance.Resource.Init();
        // Instance.Sound.Init();
        
	}

    public static void Clear()
    {

    }
    
}
