# 게임인공지능 실습 레포지토리 — GME32401

> **배재대학교** 게임공학과 · 2026-1학기
> 담당교수: 이수미 (sumilee@pcu.ac.kr)
> 엔진: **Unity 6.3 LTS (6000.3.10f1)**
> 교재: **『게임 인공지능』** (이수미) — 본 저장소는 교재 실습 워크북
> 코드 참고: *Unity Artificial Intelligence Programming, 5th Edition* (Davide Aversa, Packt)

---

## 개요 (Overview)

이 레포지토리는 게임 AI 핵심 알고리즘을 Unity 6에서 직접 구현하는 12개 실습으로 구성됩니다.
각 Lab은 독립적으로 실행 가능하며, 참고서적(Aversa)의 C# 예제를 Unity 6 API로 현대화하였습니다.

---

## 커리큘럼 (Curriculum)

| Lab | 주제 | 교재 참고 | 핵심 개념 | 주차 |
|-----|------|----------|-----------|------|
| [Lab01](Assets/Labs/Lab01_BasicAgent) | 기본 에이전트와 FSM | Chapter 2 Agent · 3 FSM | `IState`, `StateMachine`, `MonoBehaviour` | 2주 |
| [Lab02](Assets/Labs/Lab02_PatrolFSM) | 순찰 NPC FSM | Chapter 3 FSM | 다중 상태, 전환 조건, Gizmos | 3주 |
| [Lab03](Assets/Labs/Lab03_BehaviorTree) | 행동 트리 | Chapter 4 BT | Sequence, Selector, Leaf | 4주 |
| [Lab04](Assets/Labs/Lab04_UtilityAI) | 유틸리티 AI | Chapter 5 Utility | 점수 기반 의사결정, Response Curve | 5주 |
| [Lab05](Assets/Labs/Lab05_RandomProbability) | 랜덤 / 확률 | Chapter 6 Probability | Weighted Random, Seed, Noise | 6주 |
| [Lab06](Assets/Labs/Lab06_RaycastSensor) | 레이캐스트 센서 | Chapter 7 Sensors | `Physics.Raycast`, FOV, LayerMask | 7주 |
| [Lab07](Assets/Labs/Lab07_Boids) | Boids 군집 알고리즘 | Chapter 8 Flocking | 분리·정렬·결합, Reynolds Rules | 9주 |
| [Lab08](Assets/Labs/Lab08_SteeringBehaviors) | 조종 행동 | Chapter 9 Steering | Seek, Flee, Wander, Arrive | 10주 |
| [Lab09](Assets/Labs/Lab09_AStar) | A* 경로탐색 | Chapter 10 A* | `f=g+h`, OpenList, 역추적 | 11주 |
| [Lab10](Assets/Labs/Lab10_NavMesh) | NavMesh 내비게이션 | Chapter 11 NavMesh | `NavMeshAgent`, `NavMeshSurface` | 12주 |
| [Lab11](Assets/Labs/Lab11_MLAgents) | ML-Agents | Chapter 12 ML | PPO, Observation, Reward | 13주 |
| [Lab13](Assets/Labs/Lab13_GenerativeAI) | 생성형 AI / LLM NPC | Chapter 13 GenAI | LLM API, Prompt Engineering, NPC 대화 | 14주 |

---

## 심화 실습 저장소 (Advanced Standalone Projects)

메인 저장소 외에, 일부 장은 챕터별 단독 실습 프로젝트를 별도 GitHub 저장소로 제공합니다.

| 장 | 주제 | 저장소 | 설명 |
|----|------|--------|------|
| 10장 | A* | [sumilee-pcu/Astar_Rider](https://github.com/sumilee-pcu/Astar_Rider) | 공공데이터 식당 위치를 그리드에 올려 A*/Dijkstra로 배달 라이더를 자율주행시키는 **배달왕 2D** |
| 11장 | NavMesh | [sumilee-pcu/Astar_Rider_3D](https://github.com/sumilee-pcu/Astar_Rider_3D) | Unity NavMesh로 배달 트럭을 자율주행시키는 **3D** (도시 자동 생성·런타임 베이킹) |
| 12장 | ML-Agents | [sumilee-pcu/MLagent_2026](https://github.com/sumilee-pcu/MLagent_2026) | RollerAgent 강화학습(PPO) 단독 학습 환경 (`config/roller.yaml`) |
| 13장 | LLM NPC | [sumilee-pcu/LLM_NPC](https://github.com/sumilee-pcu/LLM_NPC) | 페르소나·호감도·표정을 갖춘 대화형 NPC 시뮬레이터 (클라우드/로컬 LLM, JSON 구조화 출력) |

---

## 사전 요구사항 (Requirements)

| 도구 | 버전 | 비고 |
|------|------|------|
| Unity Hub | 3.x 이상 | — |
| Unity Editor | **6000.0.36f1 LTS** | `ProjectSettings/ProjectVersion.txt` 참조 |
| Python | 3.10.x | Lab11 ML-Agents 훈련 시 필요 |
| ml-agents (pip) | 1.1.0 이상 | `pip install mlagents` |
| Git | 2.x 이상 | — |

---

## 시작하기 (Getting Started)

### 1. 포크 & 클론

```bash
# GitHub에서 이 레포지토리를 포크(Fork)한 후:
git clone https://github.com/<YOUR-ID>/gameAI2026.git
cd gameAI2026
```

### 2. Unity Hub에서 프로젝트 열기

1. Unity Hub 실행
2. **Projects → Open** 클릭
3. 클론한 `gameAI2026` 폴더 선택
4. Unity **6000.0.36f1 LTS** 버전으로 열기 (자동 설치 안내 따르기)
5. Package Manager가 패키지를 자동 설치 (인터넷 연결 필요)

### 3. ML-Agents 설치 (Lab11 전용)

```bash
pip install mlagents
# 훈련 실행
mlagents-learn Assets/Labs/Lab11_MLAgents/Config/rollerball_config.yaml --run-id=RollerBall01
```

---

## 실습 구조 (Lab Structure)

각 Lab 폴더 구성:
```
Assets/Labs/LabXX_Topic/
├── README.md          ← 실습 가이드 (한국어)
└── Scripts/           ← C# 스크립트
    ├── Core/          ← (선택) 핵심 프레임워크 클래스
    └── *.cs           ← 실습 스크립트
```

공통 유틸리티:
```
Assets/Shared/Scripts/
├── Utilities/DebugDrawer.cs     ← Gizmos 헬퍼
└── Extensions/VectorExtensions.cs
```

---

## 제출 규정 (Submission Policy)

| 항목 | 내용 |
|------|------|
| 브랜치 | `main` 또는 `lab/XX` 브랜치에 커밋 |
| 제출 방법 | e-Class 또는 GitHub 레포지토리 URL 제출 |
| 필수 파일 | `Assets/Labs/LabXX_*/` 폴더 전체 |
| 제외 파일 | `Library/`, `Temp/` (`.gitignore` 참조) |

---

## 라이선스 (License)

MIT License © 2026 이수미, 배재대학교
교재 예제 코드: © Davide Aversa, Packt Publishing (학습 목적 사용)

---

## 에셋 출처 / 크레딧 (Asset Credits)

| 에셋 | 작성자 | 출처 | 라이선스 |
|------|--------|------|----------|
| Female Character Sprite for Visual Novel (13장 LLM NPC 데모) | Sutemo | https://sutemo.itch.io/female-character | 개인·상업 사용 허용 · 크레딧 권장 · **단독 재판매 금지** |

> ⚠️ 라이선스상 원본 스프라이트(PSD)는 본 저장소에 재배포하지 않습니다. 사용 시 위 링크에서 직접 내려받으세요.
> 13장 LLM NPC 프로젝트(별도 저장소): [sumilee-pcu/LLM_NPC](https://github.com/sumilee-pcu/LLM_NPC) — `CREDITS.md` 참조

---

## 참고문헌 (References)

- Aversa, D. (2023). *Unity Artificial Intelligence Programming, 5th Edition*. Packt Publishing.
- Craig Reynolds. (1987). Flocks, herds and schools: A distributed behavioral model. *SIGGRAPH '87*.
- Russell, S., & Norvig, P. (2020). *Artificial Intelligence: A Modern Approach, 4th Ed.* Pearson.
- Sutemo. *Female Character Sprite for Visual Novel* [디지털 에셋]. itch.io. https://sutemo.itch.io/female-character (개인·상업 사용 허용)
