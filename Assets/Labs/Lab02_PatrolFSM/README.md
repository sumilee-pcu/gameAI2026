# Lab02 — 순찰 NPC FSM

> **교재 참고:** Chapter 3 유한 상태 기계(FSM) — 실전형 FSM(Patrol·Chase·Attack) | **Unity 6 업데이트 버전**

---

## 학습목표

- [ ] 다중 상태(Patrol, Chase, Attack) FSM 구현
- [ ] 거리 기반 상태 전환 조건 이해
- [ ] Waypoint 기반 순찰 로직 구현
- [ ] `OnDrawGizmos`로 감지/공격 범위 시각화
- [ ] Lab01의 `StateMachine` 재사용

---

## 상태 다이어그램

```
          플레이어 감지(10m)         공격 범위 진입(2m)
[Patrol] ──────────────▶ [Chase] ──────────────▶ [Attack]
    ◀──────────────────          ◀──────────────
       플레이어 이탈                공격 범위 이탈

   색상: 흰색              노란색              빨간색
```

---

## 씬 설정 방법

1. **New Scene** 생성
2. **Plane** 추가 (Scale: 10, 1, 10)
3. **Player** Capsule 추가, Tag → "Player" 설정
4. **PatrolPoints** Empty GameObject 추가, 자식으로 Point_0 ~ Point_3 생성
5. **Enemy** Capsule 추가
   - `NPCController.cs` 추가
   - Inspector에서 `Patrol Points` 배열에 Point_0 ~ Point_3 할당
6. **Play** 후 Player를 Enemy 근처로 이동시켜 상태 전환 확인

---

## 스크립트 구조

```
Lab02_PatrolFSM/Scripts/
├── NPCController.cs        ← NPC 메인 컨트롤러
└── States/
    ├── PatrolState.cs      ← 순찰 상태
    ├── ChaseState.cs       ← 추격 상태
    └── AttackState.cs      ← 공격 상태
```

> **Note:** `IState.cs`, `StateMachine.cs`는 `Lab01_BasicAgent/Scripts/`를 참조하거나 복사하여 사용하세요.

---

## 기본 과제

- [ ] 4개 Waypoint 순찰 동작 확인
- [ ] 10m 이내 접근 시 추격 상태 전환 확인
- [ ] 2m 이내 접근 시 공격 상태 전환 확인
- [ ] Gizmos에서 감지/공격 범위 확인
- [ ] Console 로그로 상태 전환 확인

## 심화 과제

- [ ] **Return 상태** 추가: 플레이어 이탈 후 최초 순찰 시작점으로 복귀
- [ ] **HP 시스템** 추가: 공격받으면 HP 감소, 0이 되면 비활성화
- [ ] Lab03의 `VisionSensor`와 연동: 시야각 기반 감지로 교체
