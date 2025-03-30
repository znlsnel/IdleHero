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

</details>
<details><summary>던전 생성기</summary>
  
![GenerateDungeon](https://github.com/user-attachments/assets/de1c7ae1-6919-4e94-bded-2f8c9b41025b)

- **프로시저럴 던전 생성 시스템**
  - 노드 기반의 랜덤 던전 생성
  - NavMesh 자동 생성 및 업데이트
  - 몬스터 스폰 시스템
  - 스테이지 클리어 조건 관리
  - 던전 진행도에 따른 난이도 조절

</details>
<details> <summary>기획 테이블</summary>
  
<img src="https://github.com/user-attachments/assets/fed7f7cd-a0a4-4874-99f2-6007120fde80" alt="기획 테이블" width="1000">

- **데이터 관리 시스템**
  - 엑셀 기반의 데이터 설계
  - JSON 형식으로의 자동 변환


</details>
<details> <summary>스킬 시스템</summary>

![SkillPopup](https://github.com/user-attachments/assets/da539d47-548a-47c4-b54c-10d205b70ad8)

- **다양한 스킬 구현**
  - 스킬 선택 및 강화 시스템
  - 스킬 효과 파티클 시스템


</details>
<details> <summary>상점 시스템</summary>

![Store](https://github.com/user-attachments/assets/76746564-9877-484e-b7ae-3f04102d1965)

- **아이템 구매 및 강화**
  - 골드 기반 아이템 구매
  - UI 기반 상점 인터페이스

</details>
<details><summary>캐릭터</summary><br>

![Character](https://github.com/user-attachments/assets/1b9d91e3-49bd-4ec5-a794-197b78a52e9f)

- **플레이어 시스템**
  - 상태 패턴 기반 캐릭터 제어
  - NavMesh 기반 이동 시스템
  - 자동 타겟팅 및 전투
  - 스킬 사용 시스템
  - 데미지 계산 및 전투 로직

- **몬스터 시스템**
  - AI 기반 몬스터 행동
  - NavMesh 기반 추적
  - 상태 관리 (추적, 공격, 죽음)
  - 데미지 처리 및 사망 처리
  - 스폰 및 리스폰 시스템

</details>


