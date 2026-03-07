using UnityEngine;

/// <summary>
/// Lab02: 순찰 NPC FSM 컨트롤러
/// Patrol → Chase → Attack 상태를 관리하는 NPC AI.
/// Chapter 2 AdvancedFSM 기반, Unity 6 업데이트 버전.
/// </summary>
public class NPCController : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("순찰 설정")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed    = 4f;
    [SerializeField] private float rotateSpeed  = 120f;

    [Header("감지 범위")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange    = 2f;

    [Header("참조")]
    [SerializeField] private Transform player;

    [Header("색상 피드백")]
    [SerializeField] private Color patrolColor = Color.white;
    [SerializeField] private Color chaseColor  = Color.yellow;
    [SerializeField] private Color attackColor = Color.red;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private StateMachine _stateMachine;
    private Renderer     _renderer;

    // 상태 인스턴스 (전환 시 재사용)
    private PatrolState  _patrolState;
    private ChaseState   _chaseState;
    private AttackState  _attackState;

    // =========================================================================
    // 프로퍼티 (상태 클래스에서 접근)
    // =========================================================================

    public Transform[]  PatrolPoints    => patrolPoints;
    public Transform    Player          => player;
    public float        MoveSpeed       => moveSpeed;
    public float        RotateSpeed     => rotateSpeed;
    public float        DetectionRange  => detectionRange;
    public float        AttackRange     => attackRange;

    public PatrolState  PatrolStateInstance => _patrolState;
    public ChaseState   ChaseStateInstance  => _chaseState;
    public AttackState  AttackStateInstance => _attackState;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();

        // 플레이어 자동 탐색
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            else Debug.LogWarning("[NPCController] 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void Start()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogError("[NPCController] PatrolPoints가 설정되지 않았습니다.");
            return;
        }

        _stateMachine = new StateMachine();

        _patrolState = new PatrolState(this);
        _chaseState  = new ChaseState(this);
        _attackState = new AttackState(this);

        _stateMachine.SetInitialState(_patrolState);
    }

    private void Update()
    {
        _stateMachine?.Update();
    }

    // =========================================================================
    // 공개 메서드
    // =========================================================================

    public void ChangeState(IState newState) => _stateMachine.ChangeState(newState);

    public bool IsPlayerInDetectionRange()
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= detectionRange;
    }

    public bool IsPlayerInAttackRange()
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    public void MoveToward(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.01f) return;

        dir = dir.normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        RotateToward(targetPos);
    }

    public void RotateToward(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position);
        dir.y = 0f;
        if (dir == Vector3.zero) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRot, rotateSpeed * Time.deltaTime
        );
    }

    public void SetColor(Color color)
    {
        if (_renderer != null)
            _renderer.material.color = color;
    }

    public Color PatrolColor => patrolColor;
    public Color ChaseColor  => chaseColor;
    public Color AttackColor => attackColor;

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmos()
    {
        // 감지 범위 (노란색)
        Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
        Gizmos.DrawSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 공격 범위 (빨간색)
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 순찰 경로 (청록색)
        if (patrolPoints == null) return;
        Gizmos.color = Color.cyan;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] == null) continue;
            Gizmos.DrawSphere(patrolPoints[i].position, 0.3f);
            int next = (i + 1) % patrolPoints.Length;
            if (patrolPoints[next] != null)
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[next].position);
        }
    }
}
