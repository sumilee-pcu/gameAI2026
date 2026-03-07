# Lab05 — 조종 행동 (Steering Behaviors)

> **교재 참고:** Chapter 5 (Craig Reynolds Steering Behaviors) | **Unity 6 업데이트 버전**

---

## 학습목표

- [ ] Craig Reynolds 조종 행동 4종 구현 (Seek, Flee, Arrive, Wander)
- [ ] `Rigidbody` 물리 기반 힘 적용 이해
- [ ] 조종력(Steering Force) = Desired - Velocity 공식 이해
- [ ] Inspector로 행동 실시간 전환 및 파라미터 조정

---

## 핵심 수식

```
Desired Velocity = 방향 × MaxSpeed
Steering Force   = Desired - CurrentVelocity
velocity        += SteeringForce / mass
```

| 행동 | 설명 |
|------|------|
| **Seek** | 목표를 향해 최대 속도로 이동 |
| **Flee** | 목표에서 반대 방향으로 도망 |
| **Arrive** | 목표에 가까워질수록 감속 (slowingRadius 내) |
| **Wander** | 전방 원 위의 점을 무작위로 조금씩 이동하며 배회 |

---

## 씬 설정 방법

1. **Agent Capsule** 생성, `SteeringAgent.cs` 추가, `Rigidbody` 자동 추가됨
2. **Target Sphere** 생성, `SteeringAgent`의 Target 필드에 연결
3. Inspector에서 **Behavior** 드롭다운으로 행동 전환
4. **Play** → Target을 이동시키며 행동 관찰

---

## 기본 과제

- [ ] Seek 동작 확인 (Target 따라가기)
- [ ] Flee 동작 확인 (Target에서 도망)
- [ ] Arrive 동작 확인 (감속 후 정지)
- [ ] Wander 동작 확인 (목표 없이 배회)

## 심화 과제

- [ ] **Pursuit**: 타겟의 현재 속도를 예측하여 미래 위치로 Seek
- [ ] **Evade**: Pursuit의 반대 (예측 위치에서 Flee)
- [ ] **여러 행동 조합**: 가중치를 사용하여 Seek + Wander 동시 적용
