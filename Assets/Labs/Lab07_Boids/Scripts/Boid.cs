using UnityEngine;

/// <summary>
/// Lab04: 개별 Boid 유닛.
/// Reynolds의 3가지 규칙(분리·정렬·결합)을 적용하여 군집 이동을 구현합니다.
/// </summary>
public class Boid : MonoBehaviour
{
    // =========================================================================
    // Boid 데이터 구조체 (BoidManager가 배열로 수집)
    // =========================================================================

    public struct BoidData
    {
        public Vector3 position;
        public Vector3 direction;
    }

    // =========================================================================
    // 내부 상태
    // =========================================================================

    [HideInInspector] public Vector3 velocity;

    // =========================================================================
    // 핵심 업데이트 메서드 (BoidManager에서 매 프레임 호출)
    // =========================================================================

    public void UpdateBoid(BoidData[] allBoids, BoidSettings settings, Transform target)
    {
        Vector3 flockCenter     = Vector3.zero;
        Vector3 flockDirection  = Vector3.zero;
        Vector3 separationForce = Vector3.zero;
        int     neighbourCount  = 0;
        int     avoidCount      = 0;

        for (int i = 0; i < allBoids.Length; i++)
        {
            BoidData other = allBoids[i];
            if (other.position == transform.position) continue; // 자기 자신 제외

            float dist = Vector3.Distance(transform.position, other.position);

            // 인지 반경 내 Boid 처리 (정렬 + 결합)
            if (dist < settings.perceptionRadius)
            {
                flockCenter    += other.position;
                flockDirection += other.direction;
                neighbourCount++;
            }

            // 회피 반경 내 Boid 처리 (분리)
            if (dist < settings.avoidanceRadius)
            {
                separationForce += (transform.position - other.position) / (dist + 0.001f);
                avoidCount++;
            }
        }

        // ── 가속도 계산 ──────────────────────────────────────────
        Vector3 acceleration = Vector3.zero;

        // 결합(Cohesion): 무리 중심으로 이동
        if (neighbourCount > 0)
        {
            flockCenter /= neighbourCount;
            acceleration += SteerTowards(flockCenter - transform.position) * settings.cohesionWeight;

            // 정렬(Alignment): 무리 방향과 맞춤
            acceleration += SteerTowards(flockDirection / neighbourCount) * settings.alignWeight;
        }

        // 분리(Separation): 너무 가까운 Boid에서 멀어짐
        if (avoidCount > 0)
            acceleration += SteerTowards(separationForce / avoidCount) * settings.separationWeight;

        // 경계 반발력
        if (transform.position.magnitude > settings.boundsRadius)
        {
            Vector3 toCenter = -transform.position.normalized;
            acceleration += SteerTowards(toCenter) * settings.boundsWeight;
        }

        // 목표 추적
        if (target != null)
            acceleration += SteerTowards(target.position - transform.position) * settings.targetWeight;

        // ── 속도 및 위치 업데이트 ────────────────────────────────
        velocity += acceleration * Time.deltaTime;

        float speed = velocity.magnitude;
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        velocity = velocity.normalized * speed;

        transform.position += velocity * Time.deltaTime;
        transform.forward   = velocity.normalized;
    }

    // =========================================================================
    // 헬퍼 메서드
    // =========================================================================

    /// <summary>
    /// 목표 방향으로의 조종력(Steering Force)을 계산합니다.
    /// Steering = desired - velocity (Reynolds 방식)
    /// </summary>
    private Vector3 SteerTowards(Vector3 vector)
    {
        if (vector == Vector3.zero) return Vector3.zero;
        Vector3 desired = vector.normalized * 5f; // 최대 속도 방향
        return desired - velocity;
    }

    public BoidData GetData() => new BoidData
    {
        position  = transform.position,
        direction = transform.forward
    };
}
