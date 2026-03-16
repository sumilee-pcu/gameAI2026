using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lab08: 행동 트리 기반 적 AI.
/// Chapter 9 EnemyTree 기반, 커스텀 BT 프레임워크 사용.
///
/// 트리 구조:
///   Selector
///   ├── Sequence (공격)
///   │   ├── IsInAttackRange
///   │   ├── IsPlayerVisible
///   │   └── AttackPlayer
///   ├── Sequence (추격)
///   │   ├── IsInDetectionRange
///   │   └── ChasePlayer
///   └── Patrol
/// </summary>
public class EnemyBTController : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("참조")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstacleMask;

    [Header("파라미터")]
    [SerializeField] private float detectionRange = 12f;
    [SerializeField] private float attackRange    = 2.5f;
    [SerializeField] private float moveSpeed      = 4f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("순찰")]
    [SerializeField] private float wanderRadius = 5f;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private BTNode _tree;
    private float  _attackTimer;
    private Vector3 _wanderTarget;
    private float  _wanderTimer;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Start()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        _tree = BuildTree();
        _wanderTarget = GetRandomWanderPoint();
    }

    private void Update()
    {
        _tree?.Evaluate();
        _attackTimer += Time.deltaTime;
    }

    // =========================================================================
    // 트리 구성
    // =========================================================================

    private BTNode BuildTree()
    {
        // 공격 시퀀스
        var attackSequence = new BTSequence(new List<BTNode>
        {
            IsInAttackRange(),
            IsPlayerVisible(),
            AttackPlayer()
        });

        // 추격 시퀀스
        var chaseSequence = new BTSequence(new List<BTNode>
        {
            IsInDetectionRange(),
            ChasePlayer()
        });

        // 최상위 Selector
        return new BTSelector(new List<BTNode>
        {
            attackSequence,
            chaseSequence,
            Patrol()
        });
    }

    // =========================================================================
    // 조건 노드 (Condition Leaves)
    // =========================================================================

    private BTNode IsInAttackRange() => new BTLeaf(() =>
        Vector3.Distance(transform.position, player.position) <= attackRange
            ? NodeState.Success : NodeState.Failure
    );

    private BTNode IsInDetectionRange() => new BTLeaf(() =>
        Vector3.Distance(transform.position, player.position) <= detectionRange
            ? NodeState.Success : NodeState.Failure
    );

    private BTNode IsPlayerVisible() => new BTLeaf(() =>
    {
        Vector3 dir = player.position - transform.position;
        return !Physics.Raycast(transform.position + Vector3.up, dir.normalized, dir.magnitude, obstacleMask)
            ? NodeState.Success : NodeState.Failure;
    });

    // =========================================================================
    // 액션 노드 (Action Leaves)
    // =========================================================================

    private BTNode AttackPlayer() => new BTLeaf(() =>
    {
        // 방향 주시
        Vector3 dir = (player.position - transform.position); dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(dir), 10f * Time.deltaTime);

        if (_attackTimer >= attackCooldown)
        {
            _attackTimer = 0f;
            Debug.Log("[BT] 공격!");
        }
        return NodeState.Success;
    });

    private BTNode ChasePlayer() => new BTLeaf(() =>
    {
        transform.position = Vector3.MoveTowards(
            transform.position, player.position, moveSpeed * Time.deltaTime
        );
        return NodeState.Running;
    });

    private BTNode Patrol() => new BTLeaf(() =>
    {
        // 배회 목표에 도달하면 새 목표 설정
        if (Vector3.Distance(transform.position, _wanderTarget) < 0.5f)
            _wanderTarget = GetRandomWanderPoint();

        transform.position = Vector3.MoveTowards(
            transform.position, _wanderTarget, (moveSpeed * 0.5f) * Time.deltaTime
        );
        return NodeState.Running;
    });

    // =========================================================================
    // 유틸리티
    // =========================================================================

    private Vector3 GetRandomWanderPoint()
    {
        Vector2 rand = Random.insideUnitCircle * wanderRadius;
        return transform.position + new Vector3(rand.x, 0f, rand.y);
    }

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
        Gizmos.DrawSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
