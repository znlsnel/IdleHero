using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DesignEnums;

public class ExplosionSkill : MonoBehaviour
{
    [SerializeField] private float Damage = 10;
    [SerializeField] private GameObject particle;
    [SerializeField, Range(0, 100)] private int triggerChance = 100;
    [SerializeField] private TargetSensor targetSensor;
    [SerializeField] private GameObject _attackParticle;

    private GameObject player; 
    private PlayerStatData playerStatData;
    private void Start()
    {
        player = Managers.Player.gameObject;
        playerStatData = player.GetComponent<PlayerController>().playerStatData;
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
            monster.OnDamage(playerStatData.GetStat(EStat.Damage) * (long)Damage, _attackParticle); 
        }
        Managers.Sound.Play($"Explosion/SFX_Firework_Explosion_{Random.Range(1, 4)}", 0.5f);
 
        StartCoroutine(Release(go));
    }


    private IEnumerator Release(GameObject go)
    {
        yield return new WaitForSeconds(2.0f); 
        Managers.Pool.Release(go);
    }

}
