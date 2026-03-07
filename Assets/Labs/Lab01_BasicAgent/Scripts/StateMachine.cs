using UnityEngine;

/// <summary>
/// 범용 유한 상태 기계 (Finite State Machine).
/// Generic Finite State Machine that manages IState transitions.
/// </summary>
public class StateMachine
{
    private IState _currentState;

    /// <summary>현재 활성화된 상태</summary>
    public IState CurrentState => _currentState;

    /// <summary>
    /// 초기 상태를 설정합니다. OnEnter()를 호출하며, 이전 OnExit()는 호출하지 않습니다.
    /// </summary>
    public void SetInitialState(IState state)
    {
        if (state == null)
        {
            Debug.LogWarning("[StateMachine] 초기 상태가 null입니다.");
            return;
        }
        _currentState = state;
        _currentState.OnEnter();
    }

    /// <summary>
    /// 새로운 상태로 전환합니다. 현재 상태의 OnExit() → 새 상태의 OnEnter() 순으로 호출됩니다.
    /// </summary>
    public void ChangeState(IState newState)
    {
        if (newState == null)
        {
            Debug.LogWarning("[StateMachine] 전환하려는 상태가 null입니다.");
            return;
        }
        if (newState == _currentState) return;

        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }

    /// <summary>
    /// 매 프레임 호출하여 현재 상태의 OnUpdate()를 실행합니다.
    /// MonoBehaviour의 Update()에서 호출하세요.
    /// </summary>
    public void Update()
    {
        _currentState?.OnUpdate();
    }

    /// <summary>상태 기계를 초기화합니다.</summary>
    public void Reset()
    {
        _currentState?.OnExit();
        _currentState = null;
    }
}
