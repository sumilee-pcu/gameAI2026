using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

/// <summary>
/// Lab11: ML-Agents 3.0 RollerAgent.
/// 공이 목표 큐브에 닿으면 보상을 받고 에피소드를 종료합니다.
/// Chapter 11 SphereAgent 기반, ML-Agents 3.0 API로 업데이트.
///
/// 관찰값(Observations): 8개
///   - 에이전트 localPosition (3)
///   - 목표 localPosition (3)
///   - 에이전트 linearVelocity.x (1)
///   - 에이전트 linearVelocity.z (1)
/// </summary>
public class RollerAgent : Agent
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("참조")]
    [SerializeField] private Transform target;

    [Header("물리 파라미터")]
    [SerializeField] private float forceMultiplier = 10f;

    [Header("스폰 범위")]
    [SerializeField] private float spawnRange = 4f;

    // =========================================================================
    // 내부 참조
    // =========================================================================

    private Rigidbody _rb;

    // =========================================================================
    // Agent 오버라이드
    // =========================================================================

    public override void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // 에이전트가 낙하했으면 물리 초기화
        if (transform.localPosition.y < 0)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        // 에이전트 위치 무작위 설정
        transform.localPosition = new Vector3(
            Random.Range(-spawnRange, spawnRange),
            0.5f,
            Random.Range(-spawnRange, spawnRange)
        );

        // 목표 위치 무작위 설정 (에이전트와 겹치지 않도록)
        Vector3 targetPos;
        int attempts = 0;
        do
        {
            targetPos = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0.5f,
                Random.Range(-spawnRange, spawnRange)
            );
            attempts++;
        } while (Vector3.Distance(transform.localPosition, targetPos) < 2f && attempts < 10);

        target.localPosition = targetPos;
    }

    /// <summary>
    /// 관찰값 수집 (총 8개의 float).
    /// Behavior Parameters의 Space Size = 8로 설정하세요.
    /// </summary>
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);        // 3개
        sensor.AddObservation(target.localPosition);           // 3개
        sensor.AddObservation(_rb.linearVelocity.x);           // 1개
        sensor.AddObservation(_rb.linearVelocity.z);           // 1개
        // 총 8개
    }

    /// <summary>
    /// 액션 수신 및 적용 (연속 액션 2개: X축 힘, Z축 힘).
    /// Behavior Parameters의 Continuous Actions = 2로 설정하세요.
    /// </summary>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float moveX = actionBuffers.ContinuousActions[0];
        float moveZ = actionBuffers.ContinuousActions[1];

        Vector3 force = new Vector3(moveX, 0f, moveZ) * forceMultiplier;
        _rb.AddForce(force);

        // 목표 도달 판정
        float distToTarget = Vector3.Distance(transform.localPosition, target.localPosition);
        if (distToTarget < 1.42f)
        {
            AddReward(1f);
            Debug.Log("[RollerAgent] 목표 도달! +1.0 보상");
            EndEpisode();
        }

        // 낙하 판정
        if (transform.localPosition.y < -1f)
        {
            AddReward(-1f);
            Debug.Log("[RollerAgent] 낙하! -1.0 보상");
            EndEpisode();
        }

        // 시간 패널티 (빠른 완료 유도)
        AddReward(-0.001f);
    }

    /// <summary>키보드 입력으로 직접 제어 (Heuristic 모드 테스트용)</summary>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }
}
