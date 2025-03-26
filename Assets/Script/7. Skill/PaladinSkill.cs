using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DesignEnums;

public class PaladinSkill : MonoBehaviour
{
    [SerializeField] private float Damage = 1.2f;  
    [SerializeField] private GameObject _attackParticle;
    [SerializeField] private float _speed = 180f;
    [SerializeField] private float _distance = 2f;  
    private GameObject player;
    private float _angle = 0f; // 현재 각도
    private float _yOffset = 2f;
    
    private PlayerStatData playerStatData;
    private void Start()
    {
        player = Managers.Player.gameObject;
        playerStatData = player.GetComponent<PlayerController>().playerStatData;
    }

    public void Update()
    {
        // 각도 업데이트
        _angle += _speed * Time.deltaTime;
        if (_angle >= 360f)
            _angle -= 360f;

        // 현재 각도에 따른 위치 계산
        float x = Mathf.Cos(_angle * Mathf.Deg2Rad) * _distance;
        float z = Mathf.Sin(_angle * Mathf.Deg2Rad) * _distance; 
         
        // 플레이어 위치를 기준으로 한 새로운 위치
        transform.position = player.transform.position + new Vector3(x, _yOffset, z);

        // 자전 (공전과 같은 속도로)
        transform.Rotate(Vector3.up, _speed * Time.deltaTime* 2);
    } 
 
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out MonsterController monster)) 
        {  
            monster.OnDamage((long)Mathf.Max(1, playerStatData.GetStat(EStat.Damage) * Damage / 100), _attackParticle);  
            Managers.Sound.Play($"Explosion/SFX_Firework_Explosion_{Random.Range(1, 4)}", 0.2f); 
        } 
    }


}

