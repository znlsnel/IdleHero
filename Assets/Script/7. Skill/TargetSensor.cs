using System.Collections.Generic;
using UnityEngine;

public class TargetSensor : MonoBehaviour
{
    [SerializeField] private HashSet<MonsterController> monsters = new HashSet<MonsterController>();
    public HashSet<MonsterController> Monsters => monsters;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out MonsterController monster))
        {
            monster.onDie += UnRegister; 
            monsters.Add(monster);
        }  
    } 

    private void UnRegister(GameObject monster)
    {
        if (monsters.Contains(monster.GetComponent<MonsterController>()))
        {
            monsters.Remove(monster.GetComponent<MonsterController>());
            monster.GetComponent<MonsterController>().onDie -= UnRegister;
        }
    } 

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out MonsterController monster))
        {
            UnRegister(monster.gameObject);
        }
    }
}