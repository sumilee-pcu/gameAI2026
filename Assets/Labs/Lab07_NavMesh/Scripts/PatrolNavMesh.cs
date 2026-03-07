using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Lab07: NavMeshAgent를 활용한 Waypoint 순찰 에이전트.
/// 각 Waypoint에 도착하면 waitTime초 대기 후 다음으로 이동합니다.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class PatrolNavMesh : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("순찰 설정")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float       waitTime  = 2f;
    [SerializeField] private float       patrolSpeed = 3.5f;
    [SerializeField] private bool        startFromNearest = true;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private NavMeshAgent _agent;
    private int          _waypointIndex;
    private bool         _isWaiting;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("[PatrolNavMesh] Waypoint가 설정되지 않았습니다.");
            enabled = false;
            return;
        }

        _agent.speed = patrolSpeed;
        _waypointIndex = startFromNearest ? GetNearestWaypointIndex() : 0;
        GotoCurrentWaypoint();
    }

    private void Update()
    {
        if (_isWaiting) return;
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.1f)
            StartCoroutine(WaitAndGotoNext());
    }

    // =========================================================================
    // 순찰 로직
    // =========================================================================

    private void GotoCurrentWaypoint()
    {
        _agent.isStopped = false;
        _agent.SetDestination(waypoints[_waypointIndex].position);
    }

    private IEnumerator WaitAndGotoNext()
    {
        _isWaiting = true;
        _agent.isStopped = true;
        yield return new WaitForSeconds(waitTime);
        _waypointIndex = (_waypointIndex + 1) % waypoints.Length;
        _agent.isStopped = false;
        GotoCurrentWaypoint();
        _isWaiting = false;
    }

    private int GetNearestWaypointIndex()
    {
        int   nearest = 0;
        float minDist = float.MaxValue;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            float d = Vector3.Distance(transform.position, waypoints[i].position);
            if (d < minDist) { minDist = d; nearest = i; }
        }
        return nearest;
    }

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmos()
    {
        if (waypoints == null) return;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Gizmos.color = (Application.isPlaying && i == _waypointIndex) ? Color.yellow : Color.white;
            Gizmos.DrawWireSphere(waypoints[i].position, 0.4f);
            int next = (i + 1) % waypoints.Length;
            if (waypoints[next] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[next].position);
        }
    }
}
