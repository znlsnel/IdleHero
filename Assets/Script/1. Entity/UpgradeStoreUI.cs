using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStoreUI : MonoBehaviour
{
    [SerializeField] private StatDataSO _statDataSO;
    [SerializeField] private GameObject _storeItemSlotPrefab;
    [SerializeField] private Transform _storeItemSlotParent; 
    void Start()
    {
        foreach (var statData in _statDataSO.itemDatas)
        {
            if (!statData.upgradeable) 
                continue;

            var itemSlot = Instantiate(_storeItemSlotPrefab, _storeItemSlotParent).GetComponent<StoreItemSlot>();
            itemSlot.InitSlot(statData);
        }
    } 

    public void CloseUI()
    { 
        gameObject.SetActive(false);
    }

    public void OpenUI() 
    {
        gameObject.SetActive(true);
    }
}
 