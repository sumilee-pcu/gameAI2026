# Lab12 — 펄린 노이즈 PCG

> **교재 참고:** Chapter 10 PerlinTexture.cs, CaveGenerator.cs | **Unity 6**

---

## 학습목표

- [ ] `Mathf.PerlinNoise(x, y)` 기본 사용법 이해
- [ ] 옥타브(Octaves), 퍼시스턴스(Persistence), 라쿠나리티(Lacunarity) 개념 파악
- [ ] Unity `Terrain` 컴포넌트의 높이맵 설정
- [ ] 바이옴 색상 매핑으로 지형 시각화

---

## 핵심 개념

| 개념 | 설명 | 높은 값 효과 |
|------|------|------------|
| **Scale** | 노이즈 샘플링 배율 | 더 부드러운 지형 |
| **Octaves** | 중첩되는 노이즈 레이어 수 | 더 디테일한 지형 |
| **Persistence** | 각 옥타브의 진폭 감소율 | 디테일이 강조됨 |
| **Lacunarity** | 각 옥타브의 주파수 증가율 | 더 거친 디테일 |

```
최종 높이 = Σ(PerlinNoise × amplitude) / maxAmplitude
```

---

## 씬 설정 방법

### 지형 생성 (TerrainGenerator)
1. **Terrain** GameObject 생성
2. `TerrainGenerator.cs` 추가
3. **Play** → 자동으로 노이즈 지형 생성

### 텍스처 시각화 (NoiseVisualizer)
1. **Plane** 생성
2. `NoiseVisualizer.cs` 추가
3. **Play** → 회색조 노이즈 텍스처 표시
4. `Use Biome Colors = true`로 설정하면 바이옴 색상

---

## 기본 과제

- [ ] `Scale` 값 변경(5~50)에 따른 지형 변화 관찰
- [ ] `Octaves = 1`과 `Octaves = 6` 비교
- [ ] `Use Biome Colors = true`로 바이옴 색상 확인

## 심화 과제

- [ ] **바이옴 시스템**: 두 개의 Perlin Noise를 합성하여 바이옴 경계 생성
- [ ] `[ContextMenu("Regenerate Terrain")]`로 에디터에서 지형 재생성
