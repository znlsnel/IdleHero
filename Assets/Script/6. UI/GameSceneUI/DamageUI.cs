using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUI : UI_Scene
{
    private TextMeshProUGUI text;
    private GameObject parent;
 
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void InitText(string damage, GameObject parent)
    {
        text.text = damage;  
        this.parent = parent;
    }

    public void Update()
    {
        if (parent != null)
            transform.localPosition = transform.parent.InverseTransformPoint(Camera.main.WorldToScreenPoint(parent.transform.position));
    } 
}
 