using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    [SerializeField] private int Damage = 10;
    [SerializeField] private int interval = 1;
    [SerializeField] private TargetSensor targetSensor;
    [SerializeField] private GameObject _attackParticle;
    private void Start()
    {
        targetSensor = gameObject.GetOrAddComponent<TargetSensor>();
        StartCoroutine(DamageOverTimeCoroutine());
    }
 
    void OnDisable()
    {
        StopCoroutine(DamageOverTimeCoroutine()); 
    }
 
    private IEnumerator DamageOverTimeCoroutine()
    {
        while(true)
        {
            foreach(var monster in targetSensor.Monsters)
            {
                monster.OnDamage(Damage, _attackParticle);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
