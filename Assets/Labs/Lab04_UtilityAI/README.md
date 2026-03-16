# Lab04 — 유틸리티 AI (Utility AI)

> **교재 참고:** Chapter 5 | **Unity 6**

---

## 학습목표

- [ ] 유틸리티 AI의 개념 이해 (점수 기반 의사결정)
- [ ] 각 행동(Action)에 유틸리티 함수 정의
- [ ] FSM/BT 대비 유틸리티 AI의 장단점 파악
- [ ] 여러 행동 중 가장 높은 점수의 행동 선택 구현

---

## 유틸리티 AI 구조

```
Agent
├── Action: Patrol    → score = f(distance, health, ...)
├── Action: Chase     → score = f(distance, health, ...)
├── Action: Attack    → score = f(distance, health, ...)
└── Action: Flee      → score = f(distance, health, ...)
→ 가장 높은 score의 Action 선택
```

---

## 씬 설정 방법

1. **Player Capsule** (Tag: "Player")
2. **Enemy Capsule**, `UtilityAgent.cs` 추가
3. Inspector에서 파라미터 설정
4. **Play** → 상황에 따른 행동 선택 확인

---

## 기본 과제

- [ ] 최소 3가지 행동(Patrol, Chase, Attack)에 유틸리티 함수 구현
- [ ] 현재 선택된 행동과 점수를 OnGUI로 표시
- [ ] 각 행동의 점수 변화를 시각적으로 확인

## 심화 과제

- [ ] 체력(HP)을 고려한 Flee 행동 추가
- [ ] Response Curve(곡선 함수)로 점수 계산 개선
