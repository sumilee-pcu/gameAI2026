using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lab03: 시야각(FOV) 기반 레이캐스트 시각 센서.
/// Chapter 4 Sensors 기반, Unity 6 업데이트 버전.
/// </summary>
public class VisionSensor : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("시야 설정")]
    [SerializeField] private float viewRadius   = 15f;
    [SerializeField, Range(0f, 360f)]
    private float viewAngle    = 110f;
    [SerializeField] private float eyeHeight    = 1.6f;

    [Header("레이어 마스크")]
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    [Header("성능 설정")]
    [SerializeField] private float scanInterval = 0.2f;
    [SerializeField] private int   fovSegments  = 24;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private readonly List<Transform> _visibleTargets = new List<Transform>();

    // =========================================================================
    // 프로퍼티
    // =========================================================================

    public IReadOnlyList<Transform> VisibleTargets => _visibleTargets;
    public bool HasVisibleTarget => _visibleTargets.Count > 0;
    public int  VisibleTargetCount => _visibleTargets.Count;
    public float ViewRadius => viewRadius;
    public float ViewAngle  => viewAngle;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void OnEnable()
    {
        StartCoroutine(FindVisibleTargetsRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _visibleTargets.Clear();
    }

    // =========================================================================
    // 공개 메서드
    // =========================================================================

    /// <summary>특정 타겟이 현재 보이는지 즉시 판단합니다.</summary>
    public bool CanSeeTarget(Transform target)
    {
        if (target == null) return false;

        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
        Vector3 dirToTarget = target.position - eyePos;

        // 거리 확인
        if (dirToTarget.magnitude > viewRadius) return false;

        // 시야각 확인
        if (Vector3.Angle(transform.forward, dirToTarget) > viewAngle * 0.5f) return false;

        // 장애물 레이캐스트
        if (Physics.Raycast(eyePos, dirToTarget.normalized, dirToTarget.magnitude, obstacleMask))
            return false;

        return true;
    }

    /// <summary>각도(도)를 월드 방향 벡터로 변환합니다.</summary>
    public Vector3 DirFromAngle(float angleDegrees, bool isGlobal)
    {
        if (!isGlobal)
            angleDegrees += transform.eulerAngles.y;

        return new Vector3(
            Mathf.Sin(angleDegrees * Mathf.Deg2Rad),
            0f,
            Mathf.Cos(angleDegrees * Mathf.Deg2Rad)
        );
    }

    // =========================================================================
    // 내부 탐색 로직
    // =========================================================================

    private IEnumerator FindVisibleTargetsRoutine()
    {
        var wait = new WaitForSeconds(scanInterval);
        while (true)
        {
            FindVisibleTargets();
            yield return wait;
        }
    }

    private void FindVisibleTargets()
    {
        _visibleTargets.Clear();

        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (Collider col in targetsInRadius)
        {
            Transform target = col.transform;
            Vector3 dirToTarget = (target.position - eyePos).normalized;

            // 시야각 체크
            if (Vector3.Angle(transform.forward, dirToTarget) > viewAngle * 0.5f)
                continue;

            float distToTarget = Vector3.Distance(eyePos, target.position);

            // 장애물 체크
            if (Physics.Raycast(eyePos, dirToTarget, distToTarget, obstacleMask))
                continue;

            _visibleTargets.Add(target);
        }
    }

    // =========================================================================
    // Gizmos — 시야 범위 시각화
    // =========================================================================

    private void OnDrawGizmos()
    {
        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;

        // 시야 반경 (흰색 원)
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        // FOV 호 (녹색 선분)
        Gizmos.color = Color.green;
        float angleStep = viewAngle / fovSegments;
        float startAngle = -viewAngle * 0.5f;
        Vector3 prevPoint = eyePos + DirFromAngle(startAngle, false) * viewRadius;

        for (int i = 1; i <= fovSegments; i++)
        {
            Vector3 nextPoint = eyePos + DirFromAngle(startAngle + angleStep * i, false) * viewRadius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        // 시야각 경계선
        Gizmos.DrawLine(eyePos, eyePos + DirFromAngle(-viewAngle * 0.5f, false) * viewRadius);
        Gizmos.DrawLine(eyePos, eyePos + DirFromAngle( viewAngle * 0.5f, false) * viewRadius);

        // 발견된 타겟까지 빨간선
        Gizmos.color = Color.red;
        foreach (Transform t in _visibleTargets)
            Gizmos.DrawLine(eyePos, t.position);
    }
}
