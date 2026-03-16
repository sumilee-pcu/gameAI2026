using System.Collections.Generic;

/// <summary>
/// Selector 노드 (OR 로직).
/// 하나라도 Success이면 즉시 Success를 반환합니다.
/// 모두 Failure일 때만 Failure 반환.
/// </summary>
public class BTSelector : BTNode
{
    private readonly List<BTNode> _children;

    public BTSelector(List<BTNode> children)
    {
        _children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (BTNode child in _children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Success: return state = NodeState.Success;
                case NodeState.Running: return state = NodeState.Running;
                case NodeState.Failure: continue;
            }
        }
        return state = NodeState.Failure;
    }
}
