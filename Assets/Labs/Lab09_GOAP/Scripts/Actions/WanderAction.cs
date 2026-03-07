using UnityEngine;

/// <summary>
/// Lab09: 배회(Wander) GOAP 액션 예시.
/// 전제조건 없이 항상 실행 가능하며, 완료 시 "isExploring": true 효과를 줍니다.
/// </summary>
public class WanderAction : GoapAction
{
    [SerializeField] private float wanderDuration = 3f;
    [SerializeField] private float moveSpeed      = 3f;
    [SerializeField] private float wanderRadius   = 5f;

    private float   _elapsed;
    private Vector3 _wanderTarget;
    private bool    _initialized;

    private void Awake()
    {
        // 전제조건: 없음 (항상 실행 가능)
        // 효과: isExploring = true
        AddEffect("isExploring", true);
        Cost = 1f;
    }

    public override bool Perform(GameObject agent)
    {
        if (!_initialized)
        {
            Vector2 rand    = Random.insideUnitCircle * wanderRadius;
            _wanderTarget   = agent.transform.position + new Vector3(rand.x, 0f, rand.y);
            _elapsed        = 0f;
            _initialized    = true;
        }

        // 목표 지점으로 이동
        agent.transform.position = Vector3.MoveTowards(
            agent.transform.position, _wanderTarget, moveSpeed * Time.deltaTime
        );

        _elapsed += Time.deltaTime;

        // wanderDuration 초 후 완료
        if (_elapsed >= wanderDuration)
        {
            _initialized = false;
            return true;
        }
        return false;
    }
}
