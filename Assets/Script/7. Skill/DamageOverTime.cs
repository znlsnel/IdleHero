using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static DesignEnums;
using Random = UnityEngine.Random;

public class DamageOverTime : MonoBehaviour
{
    [SerializeField] private long Damage = 10;
    [SerializeField] private int interval = 1;
    [SerializeField] private TargetSensor targetSensor;
    [SerializeField] private GameObject _attackParticle;

    private PlayerStatData playerStatData;
    private void Start()
    {
        playerStatData = Managers.Player.GetComponent<PlayerController>().playerStatData;
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
                monster.OnDamage((long)Mathf.Max(1, playerStatData.GetStat(EStat.Damage) * Damage / 100), _attackParticle);
                Managers.Sound.Play($"Explosion/SFX_Firework_Explosion_{Random.Range(1, 4)}", 0.1f);  

            }
            yield return new WaitForSeconds(interval);
        }
    }
}
