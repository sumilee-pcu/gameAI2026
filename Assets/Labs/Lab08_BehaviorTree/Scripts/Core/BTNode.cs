/// <summary>
/// Lab08: 행동 트리(Behavior Tree) 노드 기반 클래스.
/// 모든 BT 노드는 이 추상 클래스를 상속합니다.
/// </summary>
public enum NodeState { Running, Success, Failure }

public abstract class BTNode
{
    protected NodeState state;
    public NodeState State => state;

    /// <summary>노드를 평가하여 현재 상태를 반환합니다.</summary>
    public abstract NodeState Evaluate();
}
