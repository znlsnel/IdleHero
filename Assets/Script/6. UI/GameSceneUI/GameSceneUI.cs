using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider experienceSlider;
    [SerializeField] private TextMeshProUGUI coinsText;

    private PlayerStatHandler playerStatHandler;

    void Awake()
    {
        playerStatHandler = Managers.Player.GetComponent<PlayerController>().playerStatHandler;
    }
    private void Start()
    {
        playerStatHandler = new PlayerStatHandler();
        playerStatHandler.OnCheangeValue += UpdateUI;
        UpdateUI();
    }
 
    private void OnDestroy()
    {
        playerStatHandler.OnCheangeValue -= UpdateUI;
    }

 
    private void UpdateUI()
    {
        healthSlider.value = playerStatHandler.Health;
        healthSlider.maxValue = playerStatHandler.MaxHealth;
        experienceSlider.value = playerStatHandler.Experience;
        experienceSlider.maxValue = playerStatHandler.MaxExperience;
        coinsText.text = playerStatHandler.Coins.ToString();
    }

}
