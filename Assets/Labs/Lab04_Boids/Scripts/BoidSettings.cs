using UnityEngine;

/// <summary>
/// Lab04: Boids 알고리즘 파라미터 설정 (ScriptableObject).
/// Inspector에서 실시간으로 값을 조정하여 군집 행동 변화를 관찰하세요.
/// </summary>
[CreateAssetMenu(fileName = "BoidSettings", menuName = "GameAI/Boid Settings")]
public class BoidSettings : ScriptableObject
{
    [Header("스폰 설정")]
    public int   numBoids    = 100;
    public float spawnRadius = 20f;

    [Header("속도")]
    public float minSpeed = 1f;
    public float maxSpeed = 5f;

    [Header("인지 반경")]
    [Tooltip("다른 Boid를 인식하는 최대 거리")]
    public float perceptionRadius = 3f;
    [Tooltip("분리(Separation) 강제 적용 최소 거리")]
    public float avoidanceRadius  = 1f;

    [Header("행동 가중치 (0 ~ 10)")]
    [Range(0f, 10f)] public float alignWeight     = 1f;
    [Range(0f, 10f)] public float cohesionWeight  = 1f;
    [Range(0f, 10f)] public float separationWeight = 1.5f;

    [Header("경계 및 목표")]
    public float boundsRadius = 20f;
    [Range(0f, 10f)] public float boundsWeight  = 1f;
    [Range(0f, 10f)] public float targetWeight  = 1f;
}
