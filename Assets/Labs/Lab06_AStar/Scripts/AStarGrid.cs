using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lab06: A* 경로탐색 그리드.
/// 씬의 월드 공간을 노드 격자로 표현합니다.
/// (Unity 내장 Grid 컴포넌트와 이름 충돌 방지를 위해 AStarGrid로 명명)
/// </summary>
public class AStarGrid : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("그리드 설정")]
    [SerializeField] private Vector2   gridWorldSize = new Vector2(20f, 20f);
    [SerializeField] private float     nodeRadius    = 0.5f;
    [SerializeField] private LayerMask unwalkableMask;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private Node[,] _grid;
    private float   _nodeDiameter;
    private int     _gridSizeX;
    private int     _gridSizeY;

    // 경로 (Gizmos 표시용)
    [HideInInspector] public List<Node> path;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Start()
    {
        _nodeDiameter = nodeRadius * 2f;
        _gridSizeX    = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
        _gridSizeY    = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }

    // =========================================================================
    // 그리드 생성
    // =========================================================================

    private void CreateGrid()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 worldBottomLeft = transform.position
            - Vector3.right * gridWorldSize.x / 2f
            - Vector3.forward * gridWorldSize.y / 2f;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft
                    + Vector3.right  * (x * _nodeDiameter + nodeRadius)
                    + Vector3.forward * (y * _nodeDiameter + nodeRadius);

                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                _grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // =========================================================================
    // 공개 메서드
    // =========================================================================

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2f) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.z + gridWorldSize.y / 2f) / gridWorldSize.y);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
        return _grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        var neighbours = new List<Node>();

        for (int dx = -1; dx <= 1; dx++)
        for (int dy = -1; dy <= 1; dy++)
        {
            if (dx == 0 && dy == 0) continue;

            int nx = node.gridX + dx;
            int ny = node.gridY + dy;

            if (nx >= 0 && nx < _gridSizeX && ny >= 0 && ny < _gridSizeY)
                neighbours.Add(_grid[nx, ny]);
        }
        return neighbours;
    }

    public int MaxSize => _gridSizeX * _gridSizeY;

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,
            new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));

        if (_grid == null) return;

        HashSet<Node> pathSet = path != null ? new HashSet<Node>(path) : null;

        foreach (Node node in _grid)
        {
            if (pathSet != null && pathSet.Contains(node))
                Gizmos.color = Color.black;
            else
                Gizmos.color = node.walkable ? Color.white : Color.red;

            Gizmos.DrawCube(node.worldPosition,
                Vector3.one * (_nodeDiameter - 0.1f));
        }
    }
}
