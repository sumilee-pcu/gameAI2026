using UnityEngine;

/// <summary>순찰 상태: Waypoint를 순서대로 이동하며 플레이어를 감시합니다.</summary>
public class PatrolState : IState
{
    private readonly NPCController _npc;
    private int _waypointIndex;
    private const float ReachThreshold = 0.5f;

    public PatrolState(NPCController npc) { _npc = npc; }

    public void OnEnter()
    {
        Debug.Log("[NPC] 상태 진입: Patrol");
        _npc.SetColor(_npc.PatrolColor);
    }

    public void OnUpdate()
    {
        // 플레이어 감지 → Chase 전환
        if (_npc.IsPlayerInDetectionRange())
        {
            _npc.ChangeState(_npc.ChaseStateInstance);
            return;
        }

        // Waypoint 순찰
        if (_npc.PatrolPoints == null || _npc.PatrolPoints.Length == 0) return;

        Transform target = _npc.PatrolPoints[_waypointIndex];
        if (target == null) { AdvanceWaypoint(); return; }

        _npc.MoveToward(target.position);

        // 도착 판정
        if (Vector3.Distance(_npc.transform.position, target.position) < ReachThreshold)
            AdvanceWaypoint();
    }

    public void OnExit()
    {
        Debug.Log("[NPC] 상태 종료: Patrol");
    }

    private void AdvanceWaypoint()
    {
        _waypointIndex = (_waypointIndex + 1) % _npc.PatrolPoints.Length;
    }
}
