# Lab07 — NavMesh 내비게이션

> **교재 참고:** Chapter 8 | **Unity 6 AI Navigation 패키지 (com.unity.ai.navigation 2.0)**

---

## 학습목표

- [ ] Unity 6 NavMesh 시스템 구조 이해
- [ ] `NavMeshSurface` 컴포넌트로 NavMesh Bake
- [ ] `NavMeshAgent`로 목적지 이동 구현
- [ ] `NavMeshObstacle`로 동적 장애물 처리
- [ ] 순찰/감지/공격 NPC AI NavMesh 버전 구현

---

## Unity 6 변경사항 (구버전 대비)

| 구버전 (Unity 2021 이전) | Unity 6 |
|--------------------------|---------|
| Window → AI → Navigation | `NavMeshSurface` 컴포넌트 |
| Static Geometry에 자동 Bake | 원하는 오브젝트에 Surface 추가 후 Bake |
| 씬 전체 단일 NavMesh | 여러 Surface 동시 사용 가능 |

---

## 씬 설정 방법

### NavMesh Bake

1. Plane에 `NavMeshSurface` 컴포넌트 추가
2. Inspector에서 **Bake** 버튼 클릭
3. 파란색 오버레이 = NavMesh 생성 완료

### ClickToMove 실습

1. **Player Capsule** 생성, `NavMeshAgent` + `ClickToMove.cs` 추가
2. `Ground Mask` = 지형 레이어 설정
3. **Play** → 마우스 클릭으로 이동

### Patrol + Enemy 실습

1. **Waypoints** Empty GO 4개 생성
2. **Enemy Capsule**에 `EnemyNavMesh.cs` 추가
3. Inspector에서 Player, Waypoints 연결

---

## NavMeshAgent 주요 속성

| 속성 | 설명 |
|------|------|
| `SetDestination(pos)` | 목적지 설정 |
| `remainingDistance` | 목적지까지 남은 거리 |
| `isStopped` | 이동 정지/재개 |
| `speed` | 이동 속도 |
| `stoppingDistance` | 정지 거리 |

---

## 기본 과제

- [ ] NavMesh Bake 후 파란 오버레이 확인
- [ ] 클릭 이동 동작 확인
- [ ] Enemy가 Player를 감지하고 추격하는지 확인

## 심화 과제

- [ ] `NavMeshObstacle` 컴포넌트로 런타임에 장애물 추가
- [ ] **OffMeshLink**로 점프 구현 (플랫폼 간 이동)
