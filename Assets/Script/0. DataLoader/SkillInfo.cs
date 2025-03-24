using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SkillInfo
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 이름
    /// </summary>
    public string Name;

    /// <summary>
    /// 설명
    /// </summary>
    public string Description;

    /// <summary>
    /// 옵션
    /// </summary>
    public List<DesignEnums.EStat> Stat;

    /// <summary>
    /// 값 ( % )
    /// </summary>
    public List<int> StatValue;

    /// <summary>
    /// 생성할 프리팹 경로
    /// </summary>
    public List<string> EffectPrefab;

    /// <summary>
    /// 아이템 이미지 경로
    /// </summary>
    public string Image;

}
public class SkillInfoLoader
{
    public List<SkillInfo> ItemsList { get; private set; }
    public Dictionary<int, SkillInfo> ItemsDict { get; private set; }

    public SkillInfoLoader(string path = "JSON/SkillInfo")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, SkillInfo>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<SkillInfo> Items;
    }

    public SkillInfo GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public SkillInfo GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
