# Lab06 — A* 경로탐색

> **교재 참고:** Chapter 7 A* | **Unity 6 업데이트 버전**

---

## 학습목표

- [ ] A* 알고리즘 핵심 공식 `f(n) = g(n) + h(n)` 이해
- [ ] Grid 기반 노드 생성 및 장애물 감지 구현
- [ ] OpenList, ClosedList 동작 원리 파악
- [ ] RetracePath(역추적)으로 최단 경로 복원
- [ ] Gizmos로 Grid와 경로 시각화

---

## 핵심 개념

| 기호 | 의미 |
|------|------|
| `g(n)` | 시작점에서 현재 노드까지의 **실제 비용** |
| `h(n)` | 현재 노드에서 목표까지의 **추정 비용** (휴리스틱) |
| `f(n)` | `g(n) + h(n)` = 총 예상 비용 |

**거리 비용:** 직선 = 10, 대각선 = 14 (√2 × 10 ≈ 14)

---

## 씬 설정 방법

1. **GridSystem** Empty GameObject 생성:
   - `Grid.cs`, `Pathfinding.cs` 추가
   - `Grid World Size` = (20, 20), `Node Radius` = 0.5
   - `Unwalkable Mask` = Default (장애물 레이어)
2. **장애물 Cube** 여러 개 배치 (Layer: Default)
3. **Agent Capsule** 생성, `PathfindingAgent.cs` 추가
4. **Target Sphere** 생성, `PathfindingAgent`의 Target 연결
5. **Play** → Inspector에서 `Find Path` 버튼 클릭 또는 Start()에서 자동 탐색

---

## 기본 과제

- [ ] Gizmos에서 흰색(통과)/빨간색(장애물) Grid 확인
- [ ] 경로가 검정색으로 표시되는지 확인
- [ ] 에이전트가 장애물을 우회하여 이동하는지 확인

## 심화 과제

- [ ] **Weighted A***: `heuristicWeight` 변수 추가, `h(n) × weight`로 속도 vs 최적성 트레이드오프 실험
- [ ] **휴리스틱 비교**: Manhattan vs Euclidean 거리 전환 가능하도록 enum 추가
