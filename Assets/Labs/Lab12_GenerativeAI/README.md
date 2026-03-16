# Lab12 — 생성형 AI / LLM NPC (Generative AI / LLM NPC)

> **교재 참고:** Chapter 13 | **Unity 6 + Claude API / OpenAI API**

---

## 학습목표

- [ ] LLM(대형 언어 모델)을 NPC 대화에 활용하는 방법 이해
- [ ] Unity에서 외부 API 비동기 호출 구현 (UnityWebRequest)
- [ ] NPC 페르소나(Persona) 설계 및 프롬프트 엔지니어링
- [ ] 생성형 AI의 게임 AI 활용 가능성과 한계 파악

---

## 시스템 구조

```
Player Input
    ↓
Unity (UnityWebRequest)
    ↓
LLM API (Claude / OpenAI)
    ↓
NPC Response
    ↓
Unity UI (대화창)
```

---

## 씬 설정 방법

1. **NPC** 오브젝트에 `LLMNPCController.cs` 추가
2. Inspector에서 API Key 및 NPC 페르소나 설정
3. **UI Canvas** — 대화 입력창 및 응답 텍스트 연결
4. **Play** → NPC와 대화 테스트

---

## 기본 과제

- [ ] LLM API 호출 및 응답 수신 구현
- [ ] NPC 페르소나 프롬프트 작성 (캐릭터 설정)
- [ ] 대화 UI 구현 (입력창 + 응답창)

## 심화 과제

- [ ] 대화 히스토리 관리 (컨텍스트 유지)
- [ ] NPC 감정 상태에 따른 응답 스타일 변화
- [ ] 게임 세계관 정보를 System Prompt에 주입
