using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StageManager : IManager
{
    private HashSet<GameObject> _monsters = new HashSet<GameObject>();
    public GameObject[] GetMonsters()=> _monsters.ToArray();
    public DungeonGenerator dungeonGenerator {get; private set;}

    public void Init()
    {  
        dungeonGenerator = GameObject.FindFirstObjectByType<DungeonGenerator>();
    }
    

    public void Clear()
    {
 
    }

    public void SpawnMonster(float delay = 1f, int count = 50) 
    {
        Managers.Instance.StartCoroutine(SpawnMonsterCoroutine(delay, count));
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

            if(Physics.Raycast(ray, out hit, 15)) 
                randomPosition = hit.point; 
 
            if(randomPosition == Vector3.zero) 
                continue;
            
            // 몬스터 생성 및 배치
            //GameObject monster = Managers.Pool.Get("Monster/Bat"); 
            GameObject monster = Managers.Resource.Load<GameObject>("Monster/Bat");
            monster = GameObject.Instantiate(monster); 
            
            monster.transform.position = randomPosition;
                        
            _monsters.Add(monster);
            monster.GetComponent<MonsterController>().onDie += UnregisterMonster;

            // 각 몬스터 생성 사이에 약간의 딜레이 추가
            yield return null;
        }
    } 
 
    public void UnregisterMonster(GameObject monster)
    {
        _monsters.Remove(monster);
    }
}
