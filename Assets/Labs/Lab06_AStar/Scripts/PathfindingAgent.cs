using UnityEngine;

/// <summary>
/// Lab06: A* 경로를 따라 이동하는 에이전트.
/// </summary>
public class PathfindingAgent : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [SerializeField] private Transform  target;
    [SerializeField] private float      moveSpeed     = 5f;
    [SerializeField] private float      waypointThreshold = 0.4f;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private Pathfinding _pathfinding;
    private Vector3[]   _path;
    private int         _waypointIndex;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Start()
    {
        _pathfinding = FindFirstObjectByType<Pathfinding>();
        if (target != null) RequestPath();
    }

    private void Update()
    {
        if (_path == null || _waypointIndex >= _path.Length) return;

        Vector3 dest = _path[_waypointIndex];
        dest.y = transform.position.y;

        transform.position = Vector3.MoveTowards(
            transform.position, dest, moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, dest) < waypointThreshold)
            _waypointIndex++;
    }

    // =========================================================================
    // 경로 요청
    // =========================================================================

    [ContextMenu("Find Path")]
    public void RequestPath()
    {
        if (_pathfinding == null || target == null) return;
        _path = _pathfinding.FindPath(transform.position, target.position);
        _waypointIndex = 0;
    }

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmos()
    {
        if (_path == null) return;

        Gizmos.color = Color.yellow;
        for (int i = _waypointIndex; i < _path.Length; i++)
        {
            Gizmos.DrawSphere(_path[i], 0.2f);
            if (i > 0) Gizmos.DrawLine(_path[i - 1], _path[i]);
        }
    }
}
