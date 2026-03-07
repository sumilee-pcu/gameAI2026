/// <summary>
/// FSM(유한 상태 기계)의 모든 상태가 구현해야 하는 인터페이스.
/// Interface that all FSM states must implement.
/// </summary>
public interface IState
{
    /// <summary>상태 진입 시 1회 호출 (초기화, 애니메이션 시작 등)</summary>
    void OnEnter();

    /// <summary>상태가 활성화된 동안 매 프레임 호출 (주요 로직)</summary>
    void OnUpdate();

    /// <summary>상태 종료 시 1회 호출 (정리, 초기화 등)</summary>
    void OnExit();
}
