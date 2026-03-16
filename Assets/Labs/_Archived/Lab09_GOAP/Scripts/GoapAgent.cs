using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Lab09: GOAP 에이전트.
/// 매 프레임 현재 계획을 실행하고, 계획이 없으면 새로 수립합니다.
/// </summary>
public class GoapAgent : MonoBehaviour
{
    // =========================================================================
    // 세계 상태 및 목표
    // =========================================================================

    public Dictionary<string, bool> WorldState { get; } = new Dictionary<string, bool>();
    public Dictionary<string, bool> CurrentGoal { get; } = new Dictionary<string, bool>();

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private HashSet<GoapAction>  _availableActions;
    private Queue<GoapAction>    _actionQueue;
    private GoapAction           _currentAction;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Start()
    {
        // 이 오브젝트에 붙은 모든 GoapAction 컴포넌트를 수집
        _availableActions = new HashSet<GoapAction>(GetComponents<GoapAction>());

        // 초기 세계 상태 설정 (서브클래스나 Inspector에서 재정의 가능)
        InitializeWorldState();
        SetGoal("isExploring", true);
    }

    private void Update()
    {
        // 현재 액션이 없으면 새 계획 수립
        if (_currentAction == null)
        {
            if (_actionQueue == null || _actionQueue.Count == 0)
            {
                _actionQueue = GoapPlanner.Plan(gameObject, _availableActions, WorldState, CurrentGoal);

                if (_actionQueue == null)
                {
                    Debug.Log("[GoapAgent] 계획 수립 실패. 다음 프레임에 재시도.");
                    return;
                }
            }

            _currentAction = _actionQueue.Count > 0 ? _actionQueue.Dequeue() : null;
            if (_currentAction != null)
            {
                _currentAction.isRunning = true;
                Debug.Log($"[GoapAgent] 액션 시작: {_currentAction.GetType().Name}");
            }
        }

        // 현재 액션 실행
        if (_currentAction != null)
        {
            bool done = _currentAction.Perform(gameObject);
            if (done)
            {
                // 효과를 세계 상태에 적용
                _currentAction.ApplyEffects(WorldState);
                _currentAction.isRunning = false;
                Debug.Log($"[GoapAgent] 액션 완료: {_currentAction.GetType().Name}");
                _currentAction = null;
            }
        }
    }

    // =========================================================================
    // 공개 API
    // =========================================================================

    public void SetWorldState(string key, bool value) => WorldState[key] = value;

    public void SetGoal(string key, bool value)
    {
        CurrentGoal.Clear();
        CurrentGoal[key] = value;
        _actionQueue = null; // 목표가 바뀌면 재계획
        _currentAction = null;
    }

    // =========================================================================
    // 초기 세계 상태 (서브클래스에서 override)
    // =========================================================================

    protected virtual void InitializeWorldState()
    {
        WorldState["isExploring"] = false;
    }

    // =========================================================================
    // 디버그 UI
    // =========================================================================

    private void OnGUI()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"<b>현재 액션:</b> {(_currentAction != null ? _currentAction.GetType().Name : "없음")}");
        sb.AppendLine($"<b>계획 남은 수:</b> {(_actionQueue?.Count ?? 0)}");
        sb.AppendLine("<b>세계 상태:</b>");
        foreach (var kv in WorldState)
            sb.AppendLine($"  {kv.Key}: {kv.Value}");

        GUI.Label(new Rect(10, 10, 300, 300), sb.ToString());
    }
}
