# Lab05 — 랜덤 / 확률 (Random / Probability)

> **교재 참고:** Chapter 6 Random/Probability | **Unity 6**

---

## 학습목표

- [ ] 게임 AI에서 난수(Random)의 역할 이해
- [ ] 가중치 기반 확률 선택(Weighted Random) 구현
- [ ] 확률 분포를 이용한 행동 다양성 부여
- [ ] 결정론적 AI vs 확률적 AI 비교

---

## 주요 개념

| 개념 | 설명 |
|------|------|
| `Uniform Random` | 균등 분포 난수 |
| `Weighted Random` | 가중치 기반 확률 선택 |
| `Seed` | 재현 가능한 난수 생성 |
| `Noise` | Perlin Noise 등 연속 난수 |

---

## 씬 설정 방법

1. **Enemy** 오브젝트에 `ProbabilityAgent.cs` 추가
2. Inspector에서 각 행동의 가중치 설정
3. **Play** → 확률에 따른 행동 선택 확인

---

## 기본 과제

- [ ] 가중치 기반 행동 선택 구현 (Patrol 50%, Chase 30%, Attack 20%)
- [ ] 선택 결과를 Console에 출력
- [ ] 런타임에 가중치 변경 기능 추가

## 심화 과제

- [ ] 상황(체력, 거리)에 따라 동적으로 가중치 변경
- [ ] 난수 시드를 고정해 재현 가능한 시뮬레이션 구현
