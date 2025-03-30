![Typing SVG](https://readme-typing-svg.demolab.com?font=Fira+Code&size=50&pause=1000&width=435&height=80&lines=Idle+Heroooo!)
---
# 🛠️ Description
- **프로젝트 소개** <br>
  해당 프로젝트는 방치형 RPG 게임입니다. <br>
  반복적으로 생성되는 던전을 내려가며 몬스터를 해치우고, 캐릭터를 강화하며 성장하는 게임입니다 <br>
  매 스테이지마다 랜덤으로 지급되는 3개의 스킬 중 하나를 선택할 수 있습니다. <br>
  나에게 맞는 스킬을 선택하여 최대한 많이 내려가는 것이 목표입니다. <br>
<br>

- **개발 기간** : 2025.03.23 - 2025.03.26
- **개발 인원** : 1인 개발
- **사용 기술** <br>
-언어 : C#<br>
-엔진 : Unity Engine <br>
-개발 환경 : Window11 <br>
<br>

---

# 📼 플레이 영상 링크
<a href="https://www.youtube.com/shorts/zjSL14DyflI">
  <img src="https://github.com/user-attachments/assets/7166e35a-a303-419e-a461-36fb1d62f34e" alt="시연 영상" width="500">
</a>

<br><br>
---



# 📜 핵심 기능 
<details><summary>개발 프레임워크</summary>

![image](https://github.com/user-attachments/assets/7a20c389-2bc4-46d0-bf46-67628175af2e)

- **싱글톤 패턴 기반 매니저 시스템**
  - `Managers` 클래스를 통한 중앙 집중식 리소스 관리
  - 각 매니저(Resource, Pool, Sound, Stage 등)의 독립적인 기능 분리
  - 전역 접근 가능한 매니저 인스턴스 제공
  - 리소스 캐싱과 풀링을 통한 최적화

```csharp
public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    public static Managers Instance { get { Init(); return s_instance; } }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            s_instance = go.GetComponent<Managers>();
            s_instance.Init();
        }
    }
}
```

<br><br></details>
<details><summary>던전 생성기</summary>
  
![GenerateDungeon](https://github.com/user-attachments/assets/de1c7ae1-6919-4e94-bded-2f8c9b41025b)

- **프로시저럴 던전 생성 시스템**
  - 노드 기반의 랜덤 던전 생성
  - NavMesh 자동 생성 및 업데이트
  - 몬스터 스폰 시스템
  - 스테이지 클리어 조건 관리
  - 던전 진행도에 따른 난이도 조절

```csharp
public class DungeonGenerator : MonoBehaviour
{
    private void GenerateDungeon()
    {
        // 노드 생성 및 연결
        for (int i = 0; i < nodeCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject node = Instantiate(nodePrefab, randomPosition, Quaternion.identity);
            nodes.Add(node);
        }

        // NavMesh 자동 생성
        NavMeshSurface surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }
}
```

<br><br></details>
<details> <summary>기획 테이블</summary>
  
<img src="https://github.com/user-attachments/assets/fed7f7cd-a0a4-4874-99f2-6007120fde80" alt="기획 테이블" width="1000">

- **데이터 주도 설계**
  - ScriptableObject 기반의 데이터 관리
  - 캐릭터, 몬스터, 스킬 데이터 구조화
  - 스테이지별 난이도 밸런싱
  - 업그레이드 시스템 데이터 관리
  - 스킬 효과 및 밸런스 데이터

```csharp
[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObject/MonsterData")]
public class MonsterSO : ScriptableObject
{
    public string monsterName;
    public float maxHealth;
    public float attackPower;
    public float moveSpeed;
    public float attackRange;
    public float attackSpeed;
}
```

<br><br></details>
<details> <summary>스킬 시스템</summary>

![SkillPopup](https://github.com/user-attachments/assets/da539d47-548a-47c4-b54c-10d205b70ad8)

- **다양한 스킬 구현**
  - 팔라딘: 방어력 증가, 회복, 방어막
  - 드루이드: 자연의 힘, 치유, 번개
  - 마법사: 화염 폭발, 얼음 화살, 번개
  - 스킬 선택 및 강화 시스템
  - 스킬 효과 파티클 시스템

```csharp
public class PaladinSkill : MonoBehaviour
{
    private void Update()
    {
        // 플레이어 주변을 도는 스킬 효과
        _angle += _speed * Time.deltaTime;
        float x = Mathf.Cos(_angle * Mathf.Deg2Rad) * _distance;
        float z = -Mathf.Sin(_angle * Mathf.Deg2Rad) * _distance;
        transform.position = player.transform.position + new Vector3(x, _yOffset, z);
    }
}
```

<br><br></details>
<details> <summary>상점 시스템</summary>

![Store](https://github.com/user-attachments/assets/76746564-9877-484e-b7ae-3f04102d1965)

- **아이템 구매 및 강화**
  - 골드 기반 아이템 구매
  - 아이템 강화 시스템
  - 강화 확률 및 비용 관리
  - 인벤토리 시스템
  - UI 기반 상점 인터페이스

```csharp
public class UpgradeStoreUI : MonoBehaviour
{
    public void OnUpgradeButton()
    {
        if (Managers.Game.Gold >= upgradeCost)
        {
            float successRate = GetSuccessRate();
            if (Random.value <= successRate)
            {
                // 강화 성공
                itemLevel++;
                UpdateUI();
            }
            else
            {
                // 강화 실패
                itemLevel = 0;
            }
        }
    }
}
```

<br><br></details>
<details><summary>캐릭터</summary><br>

![Character](https://github.com/user-attachments/assets/1b9d91e3-49bd-4ec5-a794-197b78a52e9f)

- **플레이어 시스템**
  - 상태 패턴 기반 캐릭터 제어
  - NavMesh 기반 이동 시스템
  - 자동 타겟팅 및 전투
  - 스킬 사용 시스템
  - 데미지 계산 및 전투 로직

```csharp
public class PlayerController : MonoBehaviour
{
    private void UpdateTargetMonster()
    {
        // 가장 가까운 살아있는 몬스터를 타겟팅
        monsters.Sort((a, b) => {
            float distanceA = Vector3.Distance(transform.position, a.transform.position);
            float distanceB = Vector3.Distance(transform.position, b.transform.position);
            
            if (a.GetComponent<MonsterController>().IsDead)
                distanceA += 1000f;
            if (b.GetComponent<MonsterController>().IsDead)
                distanceB += 1000f;
                
            return distanceA.CompareTo(distanceB);
        });
        currentTarget = monsters[0];
    }
}
```

- **몬스터 시스템**
  - AI 기반 몬스터 행동
  - NavMesh 기반 추적
  - 상태 관리 (추적, 공격, 죽음)
  - 데미지 처리 및 사망 처리
  - 스폰 및 리스폰 시스템

```csharp
public class MonsterController : MonoBehaviour
{
    private void UpdateState()
    {
        switch (currentState)
        {
            case MonsterState.Trace:
                // 플레이어 추적
                agent.SetDestination(Managers.Player.transform.position);
                break;
            case MonsterState.Attack:
                // 공격 범위 내에서 공격
                if (Vector3.Distance(transform.position, Managers.Player.transform.position) <= attackRange)
                {
                    StartCoroutine(AttackRoutine());
                }
                break;
            case MonsterState.Dead:
                // 사망 처리
                OnDead();
                break;
        }
    }
}
```

<br><br></details>


