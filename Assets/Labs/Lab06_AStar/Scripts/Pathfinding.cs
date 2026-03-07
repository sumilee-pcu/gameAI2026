using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lab06: A* 경로탐색 알고리즘.
/// f(n) = g(n) + h(n) 기반으로 최단 경로를 탐색합니다.
/// </summary>
[RequireComponent(typeof(Grid))]
public class Pathfinding : MonoBehaviour
{
    // =========================================================================
    // 내부 참조
    // =========================================================================

    private Grid _grid;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    // =========================================================================
    // 공개 경로탐색 메서드
    // =========================================================================

    /// <summary>A* 알고리즘으로 경로를 탐색하여 Vector3 배열로 반환합니다.</summary>
    public Vector3[] FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode  = _grid.NodeFromWorldPoint(startPos);
        Node targetNode = _grid.NodeFromWorldPoint(targetPos);

        if (!startNode.walkable || !targetNode.walkable)
            return null;

        var openSet   = new List<Node> { startNode };
        var closedSet = new HashSet<Node>();

        // 비용 초기화
        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);
        startNode.parent = null;

        while (openSet.Count > 0)
        {
            // fCost가 가장 낮은 노드 선택
            Node current = GetLowestFCost(openSet);

            if (current == targetNode)
            {
                // 경로 역추적
                List<Node> nodePath = RetracePath(startNode, targetNode);
                _grid.path = nodePath;
                return ConvertToVector3Array(nodePath);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node neighbour in _grid.GetNeighbours(current))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                int tentativeG = current.gCost + GetDistance(current, neighbour);

                if (tentativeG < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost  = tentativeG;
                    neighbour.hCost  = GetDistance(neighbour, targetNode);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null; // 경로 없음
    }

    // =========================================================================
    // 헬퍼 메서드
    // =========================================================================

    /// <summary>
    /// 두 노드 사이의 거리 비용 계산.
    /// 대각선 이동: 14, 직선 이동: 10 (√2 ≈ 1.4 기반)
    /// </summary>
    private int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dy = Mathf.Abs(a.gridY - b.gridY);

        return dx > dy
            ? 14 * dy + 10 * (dx - dy)
            : 14 * dx + 10 * (dy - dx);
    }

    private Node GetLowestFCost(List<Node> openSet)
    {
        Node lowest = openSet[0];
        for (int i = 1; i < openSet.Count; i++)
        {
            if (openSet[i].fCost < lowest.fCost ||
               (openSet[i].fCost == lowest.fCost && openSet[i].hCost < lowest.hCost))
                lowest = openSet[i];
        }
        return lowest;
    }

    private List<Node> RetracePath(Node start, Node end)
    {
        var path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }

    private Vector3[] ConvertToVector3Array(List<Node> nodePath)
    {
        Vector3[] waypoints = new Vector3[nodePath.Count];
        for (int i = 0; i < nodePath.Count; i++)
            waypoints[i] = nodePath[i].worldPosition;
        return waypoints;
    }
}
