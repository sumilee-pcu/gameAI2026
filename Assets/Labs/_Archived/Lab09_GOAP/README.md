# Lab09 — GOAP (목표 지향 행동 계획)

> **교재 참고:** 없음 (독립 구현) | **Unity 6**

---

## 학습목표

- [ ] GOAP 아키텍처 이해 (WorldState, Action, Goal, Planner)
- [ ] 액션의 전제조건(Preconditions)과 효과(Effects) 정의
- [ ] 재귀 DFS 플래너로 최소 비용 액션 시퀀스 탐색
- [ ] FSM/BT 대비 GOAP의 유연성 이해

---

## FSM vs BT vs GOAP 비교

| 기준 | FSM | BT | GOAP |
|------|-----|----|------|
| 확장성 | 낮음 | 중간 | 높음 |
| 복잡도 | 낮음 | 중간 | 높음 |
| 동적 계획 | 없음 | 제한적 | 자동 |
| 사용 예 | 단순 NPC | 중간 AI | F.E.A.R. AI |

---

## 핵심 개념

| 개념 | 설명 | 예시 |
|------|------|------|
| WorldState | 세계의 현재 상태 (키-값) | `"hasFood": false` |
| Action | 전제조건 + 효과 + 비용 | `EatAction: 배부름=true` |
| Goal | 달성하려는 세계 상태 | `"isHungry": false` |
| Planner | 목표 달성 액션 시퀀스 탐색 | DFS/A* |

---

## 씬 설정 방법

1. **Agent** Capsule 생성
2. `GoapAgent.cs` 추가
3. `WanderAction.cs` 추가 (같은 오브젝트에)
4. **Play** → OnGUI에서 세계 상태 및 현재 액션 확인

---

## 기본 과제

- [ ] `WanderAction` 실행 후 `WorldState["isExploring"] = true` 확인
- [ ] `GoapPlanner`에 `Debug.Log` 추가하여 탐색 과정 추적

## 심화 과제

- [ ] **새 액션 추가**: `GatherFoodAction` (precondition: isHungry=true, effect: hasFood=true)
- [ ] **목표 변경 시 재계획**: 런타임에 `SetGoal()` 호출하여 다른 목표로 전환
