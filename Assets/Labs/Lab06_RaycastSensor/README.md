# Lab06 — 레이캐스트 센서

> **교재 참고:** Chapter 4 Sensors | **Unity 6 업데이트 버전**

---

## 학습목표

- [ ] `Physics.Raycast`와 `Physics.OverlapSphere` 활용
- [ ] 시야각(FOV) 기반 시각 센서 구현
- [ ] `LayerMask`를 활용한 선택적 충돌 감지
- [ ] Coroutine으로 성능 최적화 (scanInterval)
- [ ] `OnDrawGizmos`로 시야 범위 시각화

---

## 탐지 알고리즘 (4단계)

```
1. OverlapSphere   → 반경 내 모든 Target Layer 오브젝트 수집
2. 각도 체크       → Vector3.Angle로 FOV 범위 내 오브젝트 필터링
3. Raycast 차단    → 장애물이 있으면 제외
4. 결과 저장       → _visibleTargets 리스트에 추가
```

---

## 씬 설정 방법

1. **Layer 설정**: Edit → Project Settings → Tags and Layers
   - Layer 6: `Player`
   - Layer 7: `Obstacle`
2. **장애물 Cube** 여러 개 배치, Layer → `Obstacle`
3. **Player Capsule** 배치, Layer → `Player`
4. **NPC Capsule** 배치:
   - `VisionSensor.cs` 추가
   - `SensorController.cs` 추가
   - Inspector: `Target Mask` = Player, `Obstacle Mask` = Obstacle
5. **Play** → Player를 NPC 시야각 안으로 이동

---

## Inspector 설정

| 필드 | 기본값 | 설명 |
|------|--------|------|
| View Radius | 15 | 탐지 최대 거리 (m) |
| View Angle | 110 | 시야각 (도) |
| Eye Height | 1.6 | 눈 높이 오프셋 |
| Target Mask | Player | 탐지 대상 레이어 |
| Obstacle Mask | Obstacle | 차단 오브젝트 레이어 |
| Scan Interval | 0.2 | 탐색 주기 (초) |

---

## 기본 과제

- [ ] 시야각(초록색 부채꼴) Gizmos 확인
- [ ] 장애물 뒤에 있는 Player 미감지 확인
- [ ] 감지 시 NPC가 빨간색으로 변하는지 확인
- [ ] Console 로그로 감지/소실 메시지 확인

## 심화 과제

- [ ] **청각 센서** 추가: 플레이어 이동 시 `Physics.OverlapSphere`로 소리 범위 감지
- [ ] **360도 레이 시각화**: `for`문으로 5도 간격 레이를 `Debug.DrawRay`로 표시
- [ ] Lab02 NPC와 연동: `VisionSensor.HasVisibleTarget`으로 Chase 전환
