# Lab11 — ML-Agents 강화학습

> **교재 참고:** Chapter 12 Machine Learning(ML-Agents) | **ML-Agents 3.0 + Unity 6**

---

## 학습목표

- [ ] Unity ML-Agents 3.0 프레임워크 구조 이해
- [ ] `Agent` 클래스의 핵심 메서드 구현
- [ ] 관찰값(Observations), 보상(Reward), 에피소드(Episode) 개념 파악
- [ ] PPO 알고리즘으로 RollerAgent 훈련

---

## ML-Agents 3.0 주요 변경사항

| 구버전 API | ML-Agents 3.0 API |
|------------|-------------------|
| `GetComponent<Rigidbody>().velocity` | `_rb.linearVelocity` |
| `Academy.Instance` | 제거됨 |
| `MonoBehaviourSideChannel` | `SideChannelManager` |

---

## 설치 방법

### 1. Python 패키지 설치
```bash
pip install mlagents
pip install torch torchvision  # PyTorch (CPU 버전)
```

### 2. Unity Package Manager
- Package Manager → 검색: `com.unity.ml-agents` → Install
- (이미 `manifest.json`에 포함됨)

---

## 씬 설정 방법

1. **Training Area** Empty GameObject 생성 (Scale: 1)
2. **Plane** 추가 (10×10, Y=0) — 바닥
3. **Agent Sphere** 추가:
   - `Rigidbody` 추가 (Gravity: 체크)
   - `RollerAgent.cs` 추가
   - **Behavior Parameters** 컴포넌트 추가:
     - Behavior Name: `RollerBall`
     - Space Size: `8` (관찰값 수)
     - Continuous Actions: `2` (X, Z 힘)
   - **Decision Requester** 컴포넌트 추가 (Decision Period: 5)
4. **Target Cube** 추가, Agent의 Target 필드에 연결

---

## 훈련 실행

```bash
# 1. Unity Play 모드 실행 (Behavior Type: Default)
# 2. 터미널에서 실행:
mlagents-learn Assets/Labs/Lab11_MLAgents/Config/rollerball_config.yaml --run-id=RollerBall01

# TensorBoard 모니터링:
tensorboard --logdir results
```

---

## Agent 메서드 역할

| 메서드 | 호출 시점 | 역할 |
|--------|---------|------|
| `Initialize()` | 최초 1회 | 초기화 (Rigidbody 캐시) |
| `OnEpisodeBegin()` | 에피소드 시작마다 | 위치 초기화 |
| `CollectObservations()` | 매 Decision Step | 센서 데이터 수집 |
| `OnActionReceived()` | 매 Decision Step | 액션 적용 + 보상 계산 |
| `Heuristic()` | Heuristic 모드 | 키보드 수동 제어 |

---

## 기본 과제

- [ ] Heuristic 모드에서 키보드로 공 직접 조종 테스트
- [ ] 훈련 시작 후 TensorBoard에서 `cumulative_reward` 증가 확인
- [ ] 50만 스텝 후 자동으로 목표에 도달하는지 확인

## 심화 과제

- [ ] **관찰값 추가**: 목표까지의 방향 벡터 `(target.position - transform.position).normalized`를 관찰값에 추가 (Space Size: 11)
- [ ] **보상 함수 튜닝**: 목표에 가까울수록 작은 보상 추가 `AddReward(-distToTarget * 0.001f)`
