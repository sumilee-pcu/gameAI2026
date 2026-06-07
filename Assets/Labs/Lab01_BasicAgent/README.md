# Lab01 — 기본 에이전트와 FSM

> **교재 참고:** Chapter 2 Agent/PEAS · Chapter 3 유한 상태 기계(FSM) | **Unity 6 업데이트 버전**

---

## 학습목표

- [ ] `IState` 인터페이스의 역할 이해 (`OnEnter`, `OnUpdate`, `OnExit`)
- [ ] 범용 `StateMachine` 클래스 구조 파악
- [ ] `CharacterController`를 활용한 기본 이동 구현
- [ ] `Idle ↔ Move` 상태 전환 로직 이해
- [ ] 상태 변화를 색상 피드백으로 시각화

---

## 핵심 개념: 유한 상태 기계 (FSM)

```
[ Idle ] ──── 키 입력 ────▶ [ Move ]
[ Move ] ──── 입력 없음 ──▶ [ Idle ]
```

| 구성 요소 | 설명 |
|----------|------|
| **State (상태)** | 에이전트가 현재 있는 조건 (Idle, Move, Attack 등) |
| **Transition (전환)** | 한 상태에서 다른 상태로의 이동 |
| **Event (이벤트)** | 전환을 유발하는 조건 (키 입력, 거리, 타이머 등) |

---

## 씬 설정 방법

1. **New Scene** 생성 (File → New Scene → Basic 3D)
2. **Plane** 추가: GameObject → 3D Object → Plane (Scale: 5,1,5)
3. **Capsule** 추가: GameObject → 3D Object → Capsule
   - `CharacterController` 컴포넌트 추가 확인 (자동 추가됨)
   - `BasicAgent.cs` 컴포넌트 추가
4. **카메라** 위치 조정: Position (0, 10, -10), Rotation (45, 0, 0)
5. **Play** 버튼 클릭 → WASD 또는 방향키로 이동

---

## 스크립트 구조

```
Lab01_BasicAgent/Scripts/
├── IState.cs          ← 상태 인터페이스 (변경 불필요)
├── StateMachine.cs    ← FSM 관리 클래스 (변경 불필요)
└── BasicAgent.cs      ← 에이전트 MonoBehaviour (실습 대상)
```

---

## 기본 과제

- [ ] Play 모드에서 WASD 이동 확인
- [ ] 이동 시 캡슐 색상이 초록색으로 변하는지 확인
- [ ] 정지 시 흰색으로 돌아오는지 확인
- [ ] Console에 상태 전환 로그 확인

## 심화 과제

- [ ] **Stop 상태** 추가: Space 키를 누르면 빨간색으로 변하고 이동 불가
- [ ] Stop → Idle 전환 조건 구현 (Space 키 해제)
- [ ] `BasicAgent.cs`의 `TODO` 주석 위치에 구현

---

## 예상 Console 출력

```
[BasicAgent] 상태 진입: Idle
[BasicAgent] 상태 종료: Idle
[BasicAgent] 상태 진입: Move
[BasicAgent] 상태 종료: Move
[BasicAgent] 상태 진입: Idle
```
