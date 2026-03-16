using UnityEngine;

/// <summary>
/// Lab05: 조종 행동(Steering Behaviors) 에이전트.
/// Seek, Flee, Arrive, Wander 4가지 행동을 구현합니다.
/// Chapter 5 기반, Unity 6 업데이트 버전.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SteeringAgent : MonoBehaviour
{
    // =========================================================================
    // 열거형
    // =========================================================================

    public enum BehaviorType { Seek, Flee, Wander, Arrive }

    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("행동 선택")]
    [SerializeField] private BehaviorType behavior = BehaviorType.Seek;
    [SerializeField] private Transform    target;

    [Header("에이전트 파라미터")]
    [SerializeField] private float maxSpeed  = 5f;
    [SerializeField] private float maxForce  = 10f;
    [SerializeField] private float mass      = 1f;

    [Header("Arrive 파라미터")]
    [SerializeField] private float slowingRadius = 3f;

    [Header("Wander 파라미터")]
    [SerializeField] private float wanderRadius   = 1.5f;
    [SerializeField] private float wanderDistance = 3f;
    [SerializeField] private float wanderJitter   = 1f;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private Rigidbody _rb;
    private Vector3   _velocity;
    private Vector3   _wanderTarget;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX
                        | RigidbodyConstraints.FreezeRotationZ
                        | RigidbodyConstraints.FreezePositionY;
        _wanderTarget = Random.insideUnitSphere * wanderRadius;
    }

    private void FixedUpdate()
    {
        Vector3 steeringForce = Vector3.zero;
        Vector3 targetPos = target != null ? target.position : transform.position;

        switch (behavior)
        {
            case BehaviorType.Seek:   steeringForce = Seek(targetPos);          break;
            case BehaviorType.Flee:   steeringForce = Flee(targetPos);          break;
            case BehaviorType.Arrive: steeringForce = Arrive(targetPos);        break;
            case BehaviorType.Wander: steeringForce = Wander();                 break;
        }

        ApplyForce(steeringForce);
    }

    // =========================================================================
    // 조종 행동 메서드
    // =========================================================================

    /// <summary>Seek: 목표를 향해 최대 속도로 이동</summary>
    public Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desired = (targetPos - transform.position).normalized * maxSpeed;
        return Vector3.ClampMagnitude(desired - _velocity, maxForce);
    }

    /// <summary>Flee: 목표에서 최대 속도로 도망</summary>
    public Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desired = (transform.position - targetPos).normalized * maxSpeed;
        return Vector3.ClampMagnitude(desired - _velocity, maxForce);
    }

    /// <summary>Arrive: 목표에 도달할수록 속도를 줄이며 부드럽게 도착</summary>
    public Vector3 Arrive(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - transform.position;
        float dist = toTarget.magnitude;

        float speed = dist < slowingRadius
            ? maxSpeed * (dist / slowingRadius)
            : maxSpeed;

        Vector3 desired = toTarget.normalized * speed;
        return Vector3.ClampMagnitude(desired - _velocity, maxForce);
    }

    /// <summary>Wander: 원형 오프셋 방식의 자연스러운 배회</summary>
    public Vector3 Wander()
    {
        // 목표점을 원 위에서 무작위로 조금씩 이동
        _wanderTarget += new Vector3(
            Random.Range(-1f, 1f) * wanderJitter,
            0f,
            Random.Range(-1f, 1f) * wanderJitter
        );
        _wanderTarget = _wanderTarget.normalized * wanderRadius;

        // 에이전트 전방의 원 위의 목표점
        Vector3 circleCenter = transform.forward * wanderDistance;
        Vector3 wanderPoint  = circleCenter + _wanderTarget;

        return Seek(transform.position + wanderPoint);
    }

    // =========================================================================
    // 물리 적용
    // =========================================================================

    private void ApplyForce(Vector3 force)
    {
        _velocity += force / mass * Time.fixedDeltaTime;
        _velocity  = Vector3.ClampMagnitude(_velocity, maxSpeed);
        _velocity.y = 0f;

        _rb.linearVelocity = _velocity;

        if (_velocity.sqrMagnitude > 0.01f)
            transform.forward = Vector3.Lerp(transform.forward, _velocity.normalized, 10f * Time.fixedDeltaTime);
    }

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmosSelected()
    {
        // 속도 벡터
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _velocity);

        // Wander 원
        if (behavior == BehaviorType.Wander)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                transform.position + transform.forward * wanderDistance,
                wanderRadius
            );
        }

        // Arrive 감속 반경
        if (behavior == BehaviorType.Arrive && target != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
            Gizmos.DrawSphere(target.position, slowingRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(target.position, slowingRadius);
        }
    }
}
