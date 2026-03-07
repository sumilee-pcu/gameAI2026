using System.Collections.Generic;

/// <summary>
/// Sequence 노드 (AND 로직).
/// 모든 자식이 Success일 때만 Success를 반환합니다.
/// 하나라도 Failure이면 즉시 Failure 반환.
/// </summary>
public class BTSequence : BTNode
{
    private readonly List<BTNode> _children;

    public BTSequence(List<BTNode> children)
    {
        _children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (BTNode child in _children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Failure: return state = NodeState.Failure;
                case NodeState.Running: return state = NodeState.Running;
                case NodeState.Success: continue;
            }
        }
        return state = NodeState.Success;
    }
}
