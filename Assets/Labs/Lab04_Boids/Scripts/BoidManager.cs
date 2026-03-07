using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lab04: Boid 무리 관리자.
/// Boid들을 스폰하고 매 프레임 군집 계산을 실행합니다.
/// </summary>
public class BoidManager : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [SerializeField] private BoidSettings settings;
    [SerializeField] private Boid         boidPrefab;
    [SerializeField] private Transform    target; // 선택적 목표 지점

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private readonly List<Boid> _boids = new List<Boid>();

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Start()
    {
        if (settings == null)
        {
            Debug.LogError("[BoidManager] BoidSettings가 없습니다. Create → GameAI → Boid Settings");
            return;
        }
        if (boidPrefab == null)
        {
            Debug.LogError("[BoidManager] Boid Prefab이 없습니다.");
            return;
        }

        SpawnBoids();
    }

    private void Update()
    {
        if (_boids.Count == 0) return;

        // 이번 프레임의 Boid 위치/방향 스냅샷
        Boid.BoidData[] data = new Boid.BoidData[_boids.Count];
        for (int i = 0; i < _boids.Count; i++)
            data[i] = _boids[i].GetData();

        // 각 Boid 업데이트
        for (int i = 0; i < _boids.Count; i++)
            _boids[i].UpdateBoid(data, settings, target);
    }

    // =========================================================================
    // 스폰
    // =========================================================================

    private void SpawnBoids()
    {
        for (int i = 0; i < settings.numBoids; i++)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * settings.spawnRadius;
            Boid boid = Instantiate(boidPrefab, spawnPos, Random.rotation);
            boid.velocity = Random.onUnitSphere * settings.minSpeed;
            _boids.Add(boid);
        }
    }

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmos()
    {
        if (settings == null) return;

        // 경계 구
        Gizmos.color = new Color(0f, 1f, 1f, 0.1f);
        Gizmos.DrawSphere(transform.position, settings.boundsRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, settings.boundsRadius);

        // 스폰 범위
        Gizmos.color = new Color(1f, 1f, 0f, 0.05f);
        Gizmos.DrawSphere(transform.position, settings.spawnRadius);
    }
}
