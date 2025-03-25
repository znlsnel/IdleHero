using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSkill : MonoBehaviour
{
    [SerializeField] private int Damage = 10;
    [SerializeField] private GameObject particle;
    [SerializeField, Range(0, 100)] private int triggerChance = 100;
    [SerializeField] private TargetSensor targetSensor;
    private GameObject player; 
    private void Start()
    {
        player = Managers.Player.gameObject;
        player.GetComponent<PlayerController>().OnPlayerAttack += () =>Active();
        targetSensor = gameObject.GetOrAddComponent<TargetSensor>();
    }
 
    public void Active()
    {

        if (Random.Range(0, 100) >= triggerChance)
            return; 

        Vector3 position = player.transform.position;
        position += player.transform.forward * 2; 
        transform.position = position;  
 
        GameObject go = Managers.Pool.Get(particle);
        go.transform.position = position;
        foreach(var monster in targetSensor.Monsters)
        {
            monster.OnDamage(Damage); 
        }

        StartCoroutine(Release(go));
    }


    private IEnumerator Release(GameObject go)
    {
        yield return new WaitForSeconds(2.0f); 
        Managers.Pool.Release(go);
    }

}
