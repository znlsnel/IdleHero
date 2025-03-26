using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StageManager : IManager
{    private List<GameObject> _monsters = new List<GameObject>();
    private DungeonGenerator dungeonGenerator;
    private PlayerController playerController;
    public List<GameObject> GetMonsters()=> _monsters;

    public int currentStage = 0; 

    public event Action OnStageClear;
    public event Action OnChangeStage;

    public void Init()
    {  
        OnStageClear = null; 
        dungeonGenerator = GameObject.FindFirstObjectByType<DungeonGenerator>();
        playerController = GameObject.FindFirstObjectByType<PlayerController>();

        StageClear();
    }
    
    public void Clear()
    {
 
    }
 
    public void SpawnMonster(float delay = 1f, int count = 25 )   
    {
        Managers.Instance.StartCoroutine(SpawnMonsterCoroutine(delay, count + currentStage));
    } 
    private IEnumerator SpawnMonsterCoroutine(float delay, int count) 
    { 
        yield return new WaitForSeconds(delay);
        
        for(int i = 0; i < count; i++)
        {
           // NavMesh 위의 랜덤한 위치 찾기
            Vector3 randomPosition = dungeonGenerator.GetRandomPosition();
            randomPosition.y += 10f; 

            Ray ray = new Ray(randomPosition, Vector3.down);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 30))  
                randomPosition = hit.point; 
 
            if(randomPosition == Vector3.zero) 
                continue;
            
            // 몬스터 생성 및 배치
            GameObject monster = Managers.Pool.Get("Monster/Bat");  
            //GameObject monster = Managers.Resource.Load<GameObject>("Monster/Bat");
          //  monster = GameObject.Instantiate(monster);  
            
            monster.GetComponent<NavMeshAgent>().Warp(randomPosition); 
                
            _monsters.Add(monster);
            monster.GetComponent<MonsterController>().onDie += UnregisterMonster;

            // 각 몬스터 생성 사이에 약간의 딜레이 추가
          //  yield return null; 
        }
 
        playerController.SetStageMonster(_monsters); 
    } 
    private void UnregisterMonster(GameObject monster)
    {
        _monsters.Remove(monster);

        if (_monsters.Count == 0)
        {
            Managers.Sound.Play("UI/STGR_Success_Energetic_Happy", 1f);   
            StageClear();
        }
    }
    
    public void Restart()
    {
        foreach (var monster in _monsters)
        {
            monster.SetActive(false);
            Managers.Pool.Release(monster); 
        }
        _monsters.Clear();
        StageClear(true);
    }
    private void StageClear(bool Reset = false)
    {
        dungeonGenerator.GenerateDungeon(); 
        Managers.Player.transform.position = new Vector3(0, 0, 4);
        OnStageClear?.Invoke();

        if (!Reset)
        {
            Managers.UI.ShowPopupUI<SkillPopupUI>();   
            currentStage++;
            OnChangeStage?.Invoke();   
        }        
    }

    
} 
