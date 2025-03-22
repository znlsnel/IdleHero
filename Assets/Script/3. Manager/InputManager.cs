using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EPlayerInput
{
	move,
	interaction,
	LeftMouse
}

public class InputManager : Singleton<InputManager>
{
	private Dictionary<EPlayerInput, InputAction> playerInputs = new Dictionary<EPlayerInput, InputAction>();

	private KeyCode[] numKeyCodes = {
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
	};

	[Header("Input Action Assets")]
	[SerializeField] private InputActionAsset inputSystem;


	// Input Action Map  
	private InputActionMap playerInputMap;
	 

	// === Input Actions ===
	public static event Action<int> inputNumber; 
	public static InputAction GetInput(EPlayerInput type) => Instance.playerInputs[type];  
    
	private bool[] numKeyDown;
	 
	private void OnValidate()
	{ 
        BindAction(typeof(EPlayerInput));  
	}

	protected override void Awake()
	{
		base.Awake();

		BindAction(typeof(EPlayerInput));  
		SetActive(false);

		numKeyDown = new bool[numKeyCodes.Length];
	}

	private void Update()
	{
		CheckInputNumber();
	}

 
	private void BindAction(Type type)
	{
		string mapName = type.Name;
		if (mapName[0] == 'E')
			mapName = mapName.Substring(1);

		playerInputMap = inputSystem.FindActionMap(mapName);
		foreach (EPlayerInput t in Enum.GetValues(type))
			playerInputs[t] = playerInputMap.FindAction(type.ToString());
	} 

	public static void SetActive(bool active)
	{ 
		if (active)
			Instance.inputSystem.Enable();
		
		else 
			Instance.inputSystem.Disable(); 
	}




    private void CheckInputNumber()
    {
		for (int i = 0; i < numKeyCodes.Length; i++)
		{
			if (Input.GetKeyDown(numKeyCodes[i]) && !numKeyDown[i])
			{
				numKeyDown[i] = true;
				inputNumber?.Invoke(i + 1);
			}

			if (Input.GetKeyUp(numKeyCodes[i]))
			{
				numKeyDown[i] = false;
			}
		}
	}
}
