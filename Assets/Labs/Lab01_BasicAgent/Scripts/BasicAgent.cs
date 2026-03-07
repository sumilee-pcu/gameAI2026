using UnityEngine;

/// <summary>
/// Lab01: 기본 에이전트
/// IState 인터페이스와 StateMachine을 활용하여 Idle ↔ Move 상태를 전환하는 기본 에이전트.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class BasicAgent : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;

    [Header("색상 피드백")]
    [SerializeField] private Color idleColor  = Color.white;
    [SerializeField] private Color moveColor  = Color.green;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private StateMachine       _stateMachine;
    private CharacterController _cc;
    private Renderer            _renderer;
    private float               _verticalVelocity;

    // 상태 인스턴스
    private IState _idleState;
    private IState _moveState;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Awake()
    {
        _cc       = GetComponent<CharacterController>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        _stateMachine = new StateMachine();

        _idleState = new IdleState(this);
        _moveState = new MoveState(this);

        _stateMachine.SetInitialState(_idleState);
    }

    private void Update()
    {
        // 이동 입력 감지 → 상태 전환 결정
        Vector2 input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        bool isMoving = input.sqrMagnitude > 0.01f;

        if (isMoving && _stateMachine.CurrentState == _idleState)
            _stateMachine.ChangeState(_moveState);
        else if (!isMoving && _stateMachine.CurrentState == _moveState)
            _stateMachine.ChangeState(_idleState);

        _stateMachine.Update();

        ApplyGravity();
    }

    // =========================================================================
    // 공개 메서드 (상태 클래스에서 호출)
    // =========================================================================

    public void MoveAgent(Vector2 input)
    {
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        Vector3 velocity  = direction * moveSpeed + new Vector3(0f, _verticalVelocity, 0f);
        _cc.Move(velocity * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRot, rotateSpeed * Time.deltaTime
            );
        }
    }

    public void SetColor(Color color)
    {
        if (_renderer != null)
            _renderer.material.color = color;
    }

    public Color IdleColor => idleColor;
    public Color MoveColor => moveColor;

    // =========================================================================
    // 중력 처리
    // =========================================================================

    private void ApplyGravity()
    {
        if (_cc.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        _verticalVelocity += gravity * Time.deltaTime;
    }

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * 2f);
    }

    // =========================================================================
    // 내부 상태 클래스 (Inner State Classes)
    // =========================================================================

    private class IdleState : IState
    {
        private readonly BasicAgent _agent;
        public IdleState(BasicAgent agent) { _agent = agent; }

        public void OnEnter()
        {
            Debug.Log("[BasicAgent] 상태 진입: Idle");
            _agent.SetColor(_agent.IdleColor);
        }

        public void OnUpdate()
        {
            // Idle 상태: 아무 동작 없음. 상태 전환은 Update()에서 처리.
        }

        public void OnExit()
        {
            Debug.Log("[BasicAgent] 상태 종료: Idle");
        }
    }

    private class MoveState : IState
    {
        private readonly BasicAgent _agent;
        public MoveState(BasicAgent agent) { _agent = agent; }

        public void OnEnter()
        {
            Debug.Log("[BasicAgent] 상태 진입: Move");
            _agent.SetColor(_agent.MoveColor);
        }

        public void OnUpdate()
        {
            Vector2 input = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );
            _agent.MoveAgent(input);
        }

        public void OnExit()
        {
            Debug.Log("[BasicAgent] 상태 종료: Move");
        }
    }
}
