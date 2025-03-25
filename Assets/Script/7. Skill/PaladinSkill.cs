using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinSkill : MonoBehaviour
{
    private GameObject player;
    private float _speed = 180f;
    private float _distance = 3f;
    private float _angle = 0f; // 현재 각도
    private float _yOffset = 2f;
    private int Damage = 10;  

    private void Start()
    {
        player = Managers.Player.gameObject;
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
            monster.OnDamage(Damage);
        }
    }


}

