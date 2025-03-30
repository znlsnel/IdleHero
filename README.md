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

- **싱글톤 기반 매니저 시스템**
  - `Managers` 클래스를 통한 중앙 집중식 리소스 관리
  - 각 매니저(Resource, Pool, Sound, UI, Stage, Skill)의 독립적인 기능 분리
  - 전역 접근 가능한 인터페이스 제공 (Managers.Resource, Managers.Pool 등)
  - 게임 오브젝트 풀링을 통한 메모리 최적화

- **컴포넌트 기반 객체 구조**
  - UI, 로직, 데이터 계층 분리
  - GetOrAddComponent를 통한 컴포넌트 관리
  - BattleObject 기반 전투 객체 설계
  - 이벤트 기반 상호작용 구현

</details>
<details><summary>던전 생성기</summary>
  
![GenerateDungeon](https://github.com/user-attachments/assets/de1c7ae1-6919-4e94-bded-2f8c9b41025b)

- **프로시저럴 던전 생성 시스템**
  - 노드 기반의 랜덤 던전 생성
  - 동적 NavMesh 생성 및 업데이트
  - 몬스터 랜덤 스폰 시스템
  - 스테이지 클리어 조건 관리
  - 스테이지 레벨에 따른 몬스터 수 증가

- **몬스터 스폰 알고리즘**
  - NavMesh 위에 안전한 몬스터 배치
  - 레이캐스트를 통한 유효한 위치 검증
  - 스테이지마다 몬스터 수 동적 조정
  - 리스폰 시스템을 통한 재사용성 확보

</details>
<details> <summary>기획 테이블</summary>
  
<img src="https://github.com/user-attachments/assets/fed7f7cd-a0a4-4874-99f2-6007120fde80" alt="기획 테이블" width="1000">

- **데이터 관리 시스템**
  - 엑셀 기반의 데이터 설계
  - JSON 형식으로의 변환 및 로드
  - DataManager를 통한 중앙 집중식 데이터 관리
  - SkillInfo, MonsterSO 등의 구조화된 데이터

- **스크립터블 오브젝트**
  - MonsterSO를 통한 몬스터 데이터 정의
  - 타입별 데이터 캡슐화 (MonsterName, Experience, Health 등)
  - 실행 중 데이터 로드 및 활용
  - 인스펙터를 통한 직관적인 데이터 관리

</details>
<details> <summary>스킬 시스템</summary>

![SkillPopup](https://github.com/user-attachments/assets/da539d47-548a-47c4-b54c-10d205b70ad8)

- **스킬 선택 및 효과 시스템**
  - 스테이지 클리어 후 랜덤 스킬 선택 메커니즘
  - 팔라딘 스킬: 플레이어 주위를 회전하는 공격 이펙트
  - 특수 효과: 공전 및 자전 효과, 파티클 시스템
  - 데미지 계산 및 크리티컬 시스템 연동

- **스킬 매니저**
  - 중복 없는 랜덤 스킬 추출 알고리즘
  - SkillInfo를 통한 스킬 데이터 구조화
  - UI 연동을 통한 스킬 선택 및 효과 표현

</details>
<details> <summary>상점 시스템</summary>

![Store](https://github.com/user-attachments/assets/76746564-9877-484e-b7ae-3f04102d1965)

- **아이템 구매 및 강화**
  - 골드 기반 아이템 구매 시스템
  - 캐릭터 능력치 강화
  - UI 기반 상점 인터페이스


</details>
<details><summary>캐릭터</summary><br>

![Character](https://github.com/user-attachments/assets/1b9d91e3-49bd-4ec5-a794-197b78a52e9f)

- **플레이어 시스템**
  - 상태 패턴 기반 캐릭터 제어 (Idle, Move, Attack, Death)
  - NavMesh 기반 자동 이동 시스템
  - 가장 가까운 적 우선 타겟팅 로직
  - PlayerStatData를 통한 능력치 관리
  - 애니메이션 핸들러를 통한 애니메이션 제어

- **전투 시스템**
  - BattleObject 기반 데미지 시스템
  - 크리티컬 및 회피율 계산
  - 방어력 및 피해량 감소 메커니즘
  - 타겟 감지 및 공격 범위 시스템
  - 효과음 및 파티클 시스템 연동

- **몬스터 시스템**
  - 상태 기반 AI (추적, 공격, 사망)
  - NavMesh 기반 플레이어 추적
  - 몬스터 체력 및 데미지 처리
  - 사망 시 이벤트 처리 및 스테이지 진행

</details>
<details><summary>유틸리티</summary><br>

![Utility](https://github.com/user-attachments/assets/1b9d91e3-49bd-4ec5-a794-197b78a52e9f)

- **확장 메서드 시스템**
  - GameObject 확장 메서드 구현 (GetOrAddComponent)
  - UI 이벤트 바인딩 간소화 
  - 코드 재사용성 향상
  - 체인 형태의 함수 호출 지원

- **유틸 클래스**
  - 숫자 포맷팅 (큰 숫자 변환)
  - 컴포넌트 관리 기능 (GetOrAddComponent)
  - 자식 객체 검색 기능 (FindChild)
  - 게임 공통 기능 통합 관리

- **상수 및 열거형**
  - UI 이벤트 타입 정의
  - 사운드 타입 정의
  - 프로젝트 전역에서 사용되는 상수 관리
  - 타입 안전성 보장

</details>


