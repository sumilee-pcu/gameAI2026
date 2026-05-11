# Lab07 — Boids 군집 알고리즘

> **교재 참고:** Chapter 8 Flocking | **Unity 6 업데이트 버전**

---

## 학습목표

- [ ] Craig Reynolds의 Boids 3가지 규칙 구현
- [ ] `ScriptableObject`로 파라미터 분리 관리
- [ ] 매 프레임 다중 에이전트 간 상호작용 처리
- [ ] 가중치 조정으로 군집 행동 변화 관찰

---

## Reynolds의 3가지 규칙

| 규칙 | 설명 | 가중치 필드 |
|------|------|------------|
| **분리 (Separation)** | 너무 가까운 Boid에서 멀어지기 | `separationWeight` |
| **정렬 (Alignment)** | 주변 Boid와 같은 방향으로 이동 | `alignWeight` |
| **결합 (Cohesion)** | 무리 중심으로 이동 | `cohesionWeight` |

---

## 씬 설정 방법

### 자동 세팅 (권장)

Unity 메뉴에서 **`GameAI → Lab07 Boids → Setup Scene`** 한 번 클릭하면
씬·프리팹·`BoidSettings` 에셋이 자동 생성·연결되고 `Lab07_Boids.unity`로 저장됩니다.
바로 Play 버튼을 눌러 군집 이동을 확인할 수 있습니다.

### 수동 세팅 (학습용 — 원리 이해 목적)

1. **BoidManager** Empty GameObject 생성
   - `BoidManager.cs` 추가
2. **BoidSettings** 생성: Assets → Create → GameAI → Boid Settings
   - `BoidManager`의 Settings 필드에 연결
3. **Boid Prefab** 생성:
   - Capsule (Scale 0.3, 0.3, 0.6)에 `Boid.cs` 추가
   - Prefab으로 저장 → BoidManager의 Boid Prefab 필드에 연결
4. **Play** → 군집 이동 확인

---

## 파라미터 실험

| 실험 | separationWeight | alignWeight | cohesionWeight | 결과 |
|------|-----------------|-------------|----------------|------|
| 기본 | 1.5 | 1.0 | 1.0 | 자연스러운 군집 |
| 분산 | 5.0 | 0.5 | 0.5 | 흩어짐 |
| 밀집 | 0.5 | 1.5 | 3.0 | 강한 뭉침 |

---

## 기본 과제

- [ ] `numBoids = 50`으로 설정 후 군집 확인
- [ ] `separationWeight`를 0으로 설정하면 어떻게 되는지 관찰
- [ ] Gizmos에서 경계 구 확인

## 심화 과제

- [ ] **포식자(Predator)** 오브젝트 추가: Boid들이 도망가도록 역 분리력 적용
- [ ] `Job System` 또는 `Burst Compiler`로 성능 최적화 시도
