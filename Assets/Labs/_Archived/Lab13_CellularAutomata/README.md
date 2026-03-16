# Lab13 — 세포 오토마타 (Cellular Automata)

> **교재 참고:** Chapter 10 CaveGenerator.cs | **Unity 6 업데이트**

---

## 학습목표

- [ ] 세포 오토마타의 Birth/Survival 규칙 이해
- [ ] Double-buffer 방식의 동시 업데이트 구현
- [ ] 무작위 시드(Seed) 기반 절차적 생성
- [ ] `[ContextMenu]`로 에디터에서 실시간 재생성

---

## 생성 파이프라인

```
1. RandomFillMap()    → 맵을 fillPercent% 확률로 벽/공간 초기화
2. SmoothMap() × N   → 세포 오토마타 규칙 반복 적용
3. OnDrawGizmos()     → 검정(벽)/흰색(공간) Gizmos 시각화
```

---

## Conway vs Cave Generation 규칙

| 규칙 | Conway's Game of Life | Cave Generation |
|------|----------------------|----------------|
| 생존 | 이웃 2~3 | 이웃 ≥ `survivalThreshold` (4) |
| 탄생 | 이웃 = 3 | 이웃 > `birthThreshold` (4) |
| 목표 | 복잡한 생명 패턴 | 자연스러운 동굴 |

---

## 씬 설정 방법

1. **CaveGen** Empty GameObject 생성
2. `CaveGenerator.cs` 추가
3. Inspector에서 `Width = 64`, `Height = 64`, `Fill Percent = 47` 설정
4. **Play** → Gizmos 탭 활성화 → 흑백 동굴 맵 확인
5. `[ContextMenu("Regenerate Cave")]` → 에디터에서 우클릭으로 재생성

---

## 파라미터 실험

| fillPercent | smoothIterations | 결과 |
|-------------|-----------------|------|
| 40 | 5 | 넓은 개방 공간 |
| 50 | 5 | 균형 잡힌 동굴 |
| 60 | 5 | 좁은 통로 |
| 47 | 1 | 날카로운 노이즈 |
| 47 | 10 | 매우 부드러운 동굴 |

---

## 기본 과제

- [ ] `fillPercent = 47`, `smoothIterations = 5`로 동굴 생성 확인
- [ ] `useRandomSeed = true`로 다양한 동굴 생성
- [ ] `birthThreshold = 3`으로 변경 시 동굴 형태 변화 관찰

## 심화 과제

- [ ] **방 연결**: 분리된 빈 공간(방) 탐지 후 Bresenham 직선 알고리즘으로 통로 생성
- [ ] **3D 메시 생성**: Gizmos 대신 실제 Cube를 생성하거나 Marching Squares로 메시 생성
