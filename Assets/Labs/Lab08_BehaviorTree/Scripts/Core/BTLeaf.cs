using System;

/// <summary>
/// Leaf 노드 — 실제 조건 또는 액션을 담는 말단 노드.
/// 람다(Func)로 임의의 로직을 주입할 수 있습니다.
/// </summary>
public class BTLeaf : BTNode
{
    private readonly Func<NodeState> _task;

    public BTLeaf(Func<NodeState> task)
    {
        _task = task ?? throw new ArgumentNullException(nameof(task));
    }

    public override NodeState Evaluate()
    {
        return state = _task.Invoke();
    }
}
