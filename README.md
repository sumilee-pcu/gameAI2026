# 게임인공지능 실습 레포지토리 — GME32401

> **배재대학교** 게임공학과 · 2026-1학기
> 담당교수: 이수미 (sumilee@pcu.ac.kr)
> 엔진: **Unity 6 (6000.0.36f1 LTS)**
> 참고교재: *Unity Artificial Intelligence Programming, 5th Edition* (Davide Aversa, Packt)

---

## 개요 (Overview)

이 레포지토리는 게임 AI 핵심 알고리즘을 Unity 6에서 직접 구현하는 13개 실습으로 구성됩니다.
각 Lab은 독립적으로 실행 가능하며, 교재의 C# 예제를 Unity 6 API로 현대화하였습니다.

---

## 커리큘럼 (Curriculum)

| Lab | 주제 | 교재 참고 | 핵심 개념 |
|-----|------|----------|-----------|
| [Lab01](Assets/Labs/Lab01_BasicAgent) | 기본 에이전트와 FSM | Chapter 2 SimpleFSM | `IState`, `StateMachine`, `MonoBehaviour` |
| [Lab02](Assets/Labs/Lab02_PatrolFSM) | 순찰 NPC FSM | Chapter 2 AdvancedFSM | 다중 상태, 전환 조건, Gizmos |
| [Lab03](Assets/Labs/Lab03_RaycastSensor) | 레이캐스트 센서 | Chapter 4 Sensors | `Physics.Raycast`, FOV, LayerMask |
| [Lab04](Assets/Labs/Lab04_Boids) | Boids 군집 알고리즘 | Chapter 5 Flocking | 분리·정렬·결합, Reynolds Rules |
| [Lab05](Assets/Labs/Lab05_SteeringBehaviors) | 조종 행동 | Chapter 5 Steering | Seek, Flee, Wander, Arrive |
| [Lab06](Assets/Labs/Lab06_AStar) | A* 경로탐색 | Chapter 7 A* | `f=g+h`, OpenList, 역추적 |
| [Lab07](Assets/Labs/Lab07_NavMesh) | NavMesh 내비게이션 | Chapter 8 NavMesh | `NavMeshAgent`, `NavMeshSurface` |
| [Lab08](Assets/Labs/Lab08_BehaviorTree) | 행동 트리 | Chapter 9 BT | Sequence, Selector, Leaf |
| [Lab09](Assets/Labs/Lab09_GOAP) | GOAP | — | WorldState, Action, Planner |
| [Lab10](Assets/Labs/Lab10_QLearning) | Q-러닝 | — | QTable, ε-greedy, Bellman |
| [Lab11](Assets/Labs/Lab11_MLAgents) | ML-Agents | Chapter 11 ML | PPO, Observation, Reward |
| [Lab12](Assets/Labs/Lab12_PerlinNoise) | 펄린 노이즈 PCG | Chapter 10 PCG | 옥타브, 지형 생성 |
| [Lab13](Assets/Labs/Lab13_CellularAutomata) | 세포 오토마타 | Chapter 10 Cave | Birth/Survival, 동굴 생성 |

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

## 참고문헌 (References)

- Aversa, D. (2023). *Unity Artificial Intelligence Programming, 5th Edition*. Packt Publishing.
- Craig Reynolds. (1987). Flocks, herds and schools: A distributed behavioral model. *SIGGRAPH '87*.
- Russell, S., & Norvig, P. (2020). *Artificial Intelligence: A Modern Approach, 4th Ed.* Pearson.
