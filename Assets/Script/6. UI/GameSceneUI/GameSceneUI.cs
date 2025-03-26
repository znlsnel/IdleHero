using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static DesignEnums;

public class GameSceneUI : UI_Scene
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthRateText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI stageText;

    private PlayerStatData playerStatHandler;


    private void Start()
    {
        playerStatHandler = Managers.Player.GetComponent<PlayerController>().playerStatData;
        playerStatHandler.OnCheangeValue += UpdateUI; 
        UpdateUI();

        Managers.Stage.OnChangeStage += SetStageText; 
        SetStageText();
    } 
 
    private void UpdateUI()
    {
        healthSlider.value = playerStatHandler.Health; 
        healthSlider.maxValue = playerStatHandler.GetStat(EStat.MaxHealth);
        coinsText.text = Util.ConvertBigint(playerStatHandler.Coins);  
    
        healthRateText.text = $"{playerStatHandler.Health} / {playerStatHandler.GetStat(EStat.MaxHealth)}";
    }

    private void SetStageText()
    {
        stageText.text = $"Stage: {Managers.Stage.currentStage}"; 
    }

    public void OpenUpgradeStoreUI()
    { 
        Managers.UI.ShowPopupUI<UpgradeStoreUI>(); 
    }
}
