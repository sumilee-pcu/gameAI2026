using UnityEngine;

/// <summary>추격 상태: 플레이어를 향해 이동합니다.</summary>
public class ChaseState : IState
{
    private readonly NPCController _npc;

    public ChaseState(NPCController npc) { _npc = npc; }

    public void OnEnter()
    {
        Debug.Log("[NPC] 상태 진입: Chase");
        _npc.SetColor(_npc.ChaseColor);
    }

    public void OnUpdate()
    {
        if (_npc.Player == null) return;

        // 공격 범위 진입 → Attack 전환
        if (_npc.IsPlayerInAttackRange())
        {
            _npc.ChangeState(_npc.AttackStateInstance);
            return;
        }

        // 감지 범위 이탈 → Patrol 복귀
        if (!_npc.IsPlayerInDetectionRange())
        {
            _npc.ChangeState(_npc.PatrolStateInstance);
            return;
        }

        // 플레이어 추격
        _npc.MoveToward(_npc.Player.position);
    }

    public void OnExit()
    {
        Debug.Log("[NPC] 상태 종료: Chase");
    }
}
