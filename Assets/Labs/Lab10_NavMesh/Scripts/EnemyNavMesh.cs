using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Lab07: NavMesh 기반 적 AI (Patrol → Chase → Attack).
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavMesh : MonoBehaviour
{
    private enum EnemyState { Patrol, Chase, Attack }

    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("참조")]
    [SerializeField] private Transform   player;
    [SerializeField] private Transform[] patrolWaypoints;

    [Header("범위")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float attackRange    = 2f;

    [Header("속도")]
    [SerializeField] private float patrolSpeed = 3.5f;
    [SerializeField] private float chaseSpeed  = 6f;

    [Header("공격")]
    [SerializeField] private float attackCooldown = 1.5f;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private NavMeshAgent _agent;
    private EnemyState   _state;
    private int          _patrolIndex;
    private float        _attackTimer;
    private bool         _isPatrolWaiting;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    private void Start() => TransitionTo(EnemyState.Patrol);

    private void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        switch (_state)
        {
            case EnemyState.Patrol:
                if (dist <= detectionRange) TransitionTo(EnemyState.Chase);
                else UpdatePatrol();
                break;

            case EnemyState.Chase:
                if (dist <= attackRange)        TransitionTo(EnemyState.Attack);
                else if (dist > detectionRange) TransitionTo(EnemyState.Patrol);
                else _agent.SetDestination(player.position);
                break;

            case EnemyState.Attack:
                if (dist > attackRange) TransitionTo(EnemyState.Chase);
                else UpdateAttack();
                break;
        }
    }

    // =========================================================================
    // 상태 전환
    // =========================================================================

    private void TransitionTo(EnemyState newState)
    {
        _state = newState;
        switch (newState)
        {
            case EnemyState.Patrol:
                _agent.speed = patrolSpeed;
                _agent.isStopped = false;
                GotoNextWaypoint();
                break;
            case EnemyState.Chase:
                _agent.speed = chaseSpeed;
                _agent.isStopped = false;
                break;
            case EnemyState.Attack:
                _agent.isStopped = true;
                _attackTimer = attackCooldown;
                break;
        }
        Debug.Log($"[EnemyNavMesh] 상태: {newState}");
    }

    private void UpdatePatrol()
    {
        if (patrolWaypoints == null || patrolWaypoints.Length == 0) return;
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.1f)
            GotoNextWaypoint();
    }

    private void UpdateAttack()
    {
        _agent.isStopped = true;
        Vector3 dir = (player.position - transform.position); dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(dir), 10f * Time.deltaTime);

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= attackCooldown)
        {
            _attackTimer = 0f;
            Debug.Log("[EnemyNavMesh] 공격!");
        }
    }

    private void GotoNextWaypoint()
    {
        if (patrolWaypoints == null || patrolWaypoints.Length == 0) return;
        _agent.SetDestination(patrolWaypoints[_patrolIndex].position);
        _patrolIndex = (_patrolIndex + 1) % patrolWaypoints.Length;
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
