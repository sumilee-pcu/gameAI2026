using UnityEngine;

/// <summary>공격 상태: 플레이어를 바라보며 일정 간격으로 공격합니다.</summary>
public class AttackState : IState
{
    private readonly NPCController _npc;
    private const float AttackCooldown = 1f;
    private float _attackTimer;

    public AttackState(NPCController npc) { _npc = npc; }

    public void OnEnter()
    {
        Debug.Log("[NPC] 상태 진입: Attack");
        _npc.SetColor(_npc.AttackColor);
        _attackTimer = AttackCooldown; // 진입 즉시 첫 공격
    }

    public void OnUpdate()
    {
        if (_npc.Player == null) return;

        // 공격 범위 이탈 → Chase 복귀
        if (!_npc.IsPlayerInAttackRange())
        {
            _npc.ChangeState(_npc.ChaseStateInstance);
            return;
        }

        // 플레이어 방향 주시
        _npc.RotateToward(_npc.Player.position);

        // 공격 쿨다운
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooldown)
        {
            _attackTimer = 0f;
            PerformAttack();
        }
    }

    public void OnExit()
    {
        Debug.Log("[NPC] 상태 종료: Attack");
        _attackTimer = 0f;
    }

    private void PerformAttack()
    {
        Debug.Log("[NPC] 공격!");
        // TODO: 플레이어에게 데미지 적용
        // PlayerHealth health = _npc.Player.GetComponent<PlayerHealth>();
        // if (health != null) health.TakeDamage(10);
    }
}
