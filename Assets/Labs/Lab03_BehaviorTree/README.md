# Lab03 — 행동 트리 (Behavior Tree)

> **교재 참고:** Chapter 4 Behavior Tree | **커스텀 BT 프레임워크, Unity 6**

---

## 학습목표

- [ ] 행동 트리 구조 이해 (Composite, Leaf 노드)
- [ ] `BTSequence`(AND)와 `BTSelector`(OR) 차이 파악
- [ ] 람다(Func)로 조건/액션 노드 구현
- [ ] FSM 대비 BT의 확장성 이해

---

## 트리 구조

```
Selector (OR)
├── Sequence (AND) ← 공격 조건
│   ├── IsInAttackRange  ← Condition
│   ├── IsPlayerVisible  ← Condition (Raycast)
│   └── AttackPlayer     ← Action
├── Sequence (AND) ← 추격 조건
│   ├── IsInDetectionRange ← Condition
│   └── ChasePlayer        ← Action
└── Patrol               ← Action (기본 동작)
```

---

## 노드 반환값

| 상태 | 의미 |
|------|------|
| `Success` | 성공적으로 완료 |
| `Failure` | 실패 (조건 불충족 등) |
| `Running` | 아직 진행 중 (다음 프레임에도 계속) |

---

## 씬 설정 방법

1. **Player Capsule** (Tag: "Player")
2. **Enemy Capsule**, `EnemyBTController.cs` 추가
3. Inspector에서 Player 연결, Obstacle Mask 설정
4. **Play** → 거리에 따라 Patrol → Chase → Attack 전환 확인

---

## 기본 과제

- [ ] 트리 평가 순서 Console로 추적 (각 노드에 `Debug.Log` 추가)
- [ ] 공격 시 빨간색, 추격 시 노란색으로 색상 변경 추가
- [ ] `IsPlayerVisible` 조건에 시야각 추가

## 심화 과제

- [ ] **체력 조건** 추가: HP < 30%이면 도망(Flee) 노드 Selector 최상단에 추가
- [ ] BT 시각화: OnGUI로 현재 활성 노드 표시
