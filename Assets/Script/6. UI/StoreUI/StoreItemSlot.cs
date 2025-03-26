using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DesignEnums;

public class StoreItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _curStatText;
    [SerializeField] private TextMeshProUGUI _nxtStatText;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _buyButton;
    [SerializeField] private EStat statType;

    private PlayerStatData playerStatData; 
    private StatData statData;
 
    private void Awake()
    {

        playerStatData = Managers.Player.GetComponent<PlayerController>().playerStatData;

    }  
  
    public void InitSlot(StatData statData)
    {
        this.statData = statData;
        _icon.sprite = statData.icon;
        statType = statData.statType;
        _nameText.text = statData.statName;

        UpdateStatInfo();
        _buyButton.onClick.AddListener(Buy); 
    } 

    private void UpdateStatInfo()
    {
        int level = playerStatData.GetLevel(statType); 
 
        _levelText.text = (level).ToString(); 
        _priceText.text = level == 50 ? "MAX" : Util.ConvertBigint(playerStatData.GetLevelPrice(level+1));
        
        _curStatText.text = Util.ConvertBigint(playerStatData.GetStat(statType));
        _nxtStatText.text = level == 50 ? "MAX" : Util.ConvertBigint(GetNextStatValue());  
    }

    private void Buy()
    {
       int level = playerStatData.GetLevel(statType);
       if (playerStatData.Coins < playerStatData.GetLevelPrice(level+1))
       {
        Debug.Log("돈이 부족합니다.");
        return;
       }
        if (level == 50)
        {
            Debug.Log("MAX"); 
            return;
        }

        playerStatData.Coins -= playerStatData.GetLevelPrice(level+1);
        playerStatData.SetLevel(statType, level+1);

        playerStatData.ModifyStat(statType, GetNextStatValue(), true);  
        
        UpdateStatInfo();
       
        Debug.Log($"Button Clicked : {statType}"); 
    }

    private long GetNextStatValue() 
    { 
        long statValue = playerStatData.GetStat(statType);

    
        long plusStatValue = (long)Mathf.Max(1, (int)(statValue * statData.upgradeRate));     
        long nextStatValue = statValue + plusStatValue; 

        if (statData.plusStat)
        {
            nextStatValue = statValue + (long)statData.upgradeRate; 
        } 

        return nextStatValue;
    }
}
