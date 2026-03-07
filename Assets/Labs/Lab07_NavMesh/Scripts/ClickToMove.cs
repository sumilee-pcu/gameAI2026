using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Lab07: 마우스 클릭 위치로 NavMeshAgent를 이동시킵니다.
/// Chapter 8 기반, Unity 6 AI Navigation 패키지 사용.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("클릭 효과")]
    [SerializeField] private ParticleSystem clickEffect; // 선택적
    [SerializeField] private LayerMask      groundMask;  // 클릭 가능 레이어

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private NavMeshAgent _agent;
    private Camera       _camera;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Awake()
    {
        _agent  = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryMoveToClick();
    }

    // =========================================================================
    // 이동 처리
    // =========================================================================

    private void TryMoveToClick()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            return;

        // NavMesh 위의 가장 가까운 점으로 보정
        if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
        {
            _agent.SetDestination(navHit.position);
            ShowClickEffect(navHit.position);
            Debug.Log($"[ClickToMove] 목표: {navHit.position}");
        }
    }

    private void ShowClickEffect(Vector3 position)
    {
        if (clickEffect == null) return;
        clickEffect.transform.position = position + Vector3.up * 0.05f;
        clickEffect.Play();
    }

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || _agent == null) return;

        // 에이전트에서 목적지까지 경로 표시
        if (_agent.hasPath)
        {
            Gizmos.color = Color.cyan;
            Vector3[] corners = _agent.path.corners;
            for (int i = 0; i < corners.Length - 1; i++)
                Gizmos.DrawLine(corners[i], corners[i + 1]);
        }
    }
}
