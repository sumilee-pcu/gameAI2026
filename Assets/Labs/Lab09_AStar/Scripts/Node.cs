using UnityEngine;

/// <summary>Lab06: A* 경로탐색 그리드 노드</summary>
public class Node
{
    public bool    walkable;
    public Vector3 worldPosition;
    public int     gridX;
    public int     gridY;

    public int gCost; // 시작점에서 현재 노드까지의 실제 비용
    public int hCost; // 현재 노드에서 목표까지의 추정 비용(휴리스틱)
    public int fCost => gCost + hCost; // 총 비용

    public Node parent; // 경로 역추적용 부모 노드

    public Node(bool walkable, Vector3 worldPos, int gridX, int gridY)
    {
        this.walkable      = walkable;
        this.worldPosition = worldPos;
        this.gridX         = gridX;
        this.gridY         = gridY;
    }
}
