using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSceneUI : UI_Scene
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthRateText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI stageText;

    private PlayerStatHandler playerStatHandler;


    private void Start()
    {
        playerStatHandler = Managers.Player.GetComponent<PlayerController>().playerStatHandler;
        playerStatHandler.OnCheangeValue += UpdateUI; 
        UpdateUI();

        Managers.Stage.OnChangeStage += SetStageText; 
        SetStageText();
    } 
 
    private void OnDestroy() 
    {
        playerStatHandler.OnCheangeValue -= UpdateUI; 
        Managers.Stage.OnChangeStage -= SetStageText;
    }

    private void UpdateUI()
    {
        healthSlider.value = playerStatHandler.Health; 
        healthSlider.maxValue = playerStatHandler.MaxHealth;
        coinsText.text = playerStatHandler.Coins.ToString();
    
        float temp = healthSlider.value / healthSlider.maxValue;
        healthRateText.text = $"{(int)(temp * 100)}%";
    }

    private void SetStageText()
    {
        stageText.text = $"Stage: {Managers.Stage.currentStage}"; 
    }
}
