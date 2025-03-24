using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : IManager
{
    private HashSet<GameObject> _monsters = new HashSet<GameObject>();
    public GameObject[] GetMonsters()=> _monsters.ToArray();


    public void Init()
    { 
        GameObject go = Managers.Pool.Get("Monster/Bat"); 
    }
    

    public void Clear()
    {

    }

    public void RegisterMonster(GameObject monster)
    {
        _monsters.Add(monster);
    }

    public void UnregisterMonster(GameObject monster)
    {
        _monsters.Remove(monster);
    }


}
