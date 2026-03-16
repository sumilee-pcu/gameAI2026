using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lab09: GOAP 액션 기반 클래스.
/// 전제조건(Preconditions)과 효과(Effects)를 정의하고 실제 동작을 구현합니다.
/// </summary>
public abstract class GoapAction : MonoBehaviour
{
    // =========================================================================
    // GOAP 데이터
    // =========================================================================

    /// <summary>이 액션이 실행되기 위해 필요한 세계 상태 조건</summary>
    public Dictionary<string, bool> Preconditions { get; } = new Dictionary<string, bool>();

    /// <summary>이 액션이 완료된 후 세계 상태에 적용되는 변화</summary>
    public Dictionary<string, bool> Effects { get; } = new Dictionary<string, bool>();

    /// <summary>액션의 비용 (플래너가 최소 비용 경로를 선택할 때 사용)</summary>
    public float Cost = 1f;

    // =========================================================================
    // 런타임 상태
    // =========================================================================

    [HideInInspector] public bool isRunning;

    // =========================================================================
    // 추상 메서드
    // =========================================================================

    /// <summary>
    /// 절차적 전제조건 확인 (동적 조건, 예: 특정 오브젝트가 주변에 있는지).
    /// 정적 Preconditions 외에 런타임에 추가로 검사합니다.
    /// </summary>
    public virtual bool CheckProceduralPrecondition(GameObject agent) => true;

    /// <summary>
    /// 액션 실행. 완료되면 true, 아직 진행 중이면 false를 반환합니다.
    /// GoapAgent의 Update에서 매 프레임 호출됩니다.
    /// </summary>
    public abstract bool Perform(GameObject agent);

    // =========================================================================
    // 헬퍼 메서드
    // =========================================================================

    protected void AddPrecondition(string key, bool value) => Preconditions[key] = value;
    protected void AddEffect(string key, bool value)        => Effects[key] = value;

    /// <summary>
    /// 현재 세계 상태가 이 액션의 Preconditions를 모두 만족하는지 확인합니다.
    /// </summary>
    public bool ArePreconditionsMet(Dictionary<string, bool> worldState)
    {
        foreach (var kv in Preconditions)
        {
            if (!worldState.TryGetValue(kv.Key, out bool val) || val != kv.Value)
                return false;
        }
        return true;
    }

    /// <summary>이 액션의 Effects를 worldState에 적용합니다.</summary>
    public void ApplyEffects(Dictionary<string, bool> worldState)
    {
        foreach (var kv in Effects)
            worldState[kv.Key] = kv.Value;
    }
}
