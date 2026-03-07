using UnityEngine;

/// <summary>
/// Lab13: 세포 오토마타 기반 동굴 맵 생성기.
/// Chapter 10 CaveGenerator.cs 기반, Unity 6 업데이트.
/// Birth/Survival threshold로 자연스러운 동굴을 절차적으로 생성합니다.
/// </summary>
public class CaveGenerator : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("맵 크기")]
    [SerializeField] private int width  = 64;
    [SerializeField] private int height = 64;

    [Header("초기화")]
    [SerializeField, Range(0, 100)]
    private int fillPercent = 47;
    [SerializeField] private bool   useRandomSeed = true;
    [SerializeField] private string seed          = "GameAI2026";

    [Header("세포 오토마타 규칙")]
    [SerializeField] private int smoothIterations = 5;
    [Tooltip("생존 조건: 살아있는 이웃 수가 이 값 이상이면 벽 유지")]
    [SerializeField, Range(0, 8)] private int survivalThreshold = 4;
    [Tooltip("탄생 조건: 살아있는 이웃 수가 이 값 이상이면 벽으로 변환")]
    [SerializeField, Range(0, 8)] private int birthThreshold    = 4;

    [Header("셀 시각화")]
    [SerializeField] private float cellSize = 1f;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private int[,] _map; // 0 = 빈 공간, 1 = 벽

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Start()
    {
        GenerateCave();
    }

    // =========================================================================
    // 동굴 생성 파이프라인
    // =========================================================================

    [ContextMenu("Regenerate Cave")]
    public void GenerateCave()
    {
        _map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < smoothIterations; i++)
            SmoothMap();

        Debug.Log($"[CaveGenerator] 동굴 생성 완료 ({width}×{height}, 채움률:{fillPercent}%)");
    }

    /// <summary>초기 맵을 무작위로 채웁니다.</summary>
    private void RandomFillMap()
    {
        int seedValue = useRandomSeed
            ? (int)System.DateTime.Now.Ticks
            : seed.GetHashCode();

        var rng = new System.Random(seedValue);

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            // 경계는 항상 벽
            if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
            {
                _map[x, y] = 1;
                continue;
            }
            _map[x, y] = rng.Next(0, 100) < fillPercent ? 1 : 0;
        }
    }

    /// <summary>1회의 세포 오토마타 스텝을 적용합니다 (double-buffer 방식).</summary>
    private void SmoothMap()
    {
        int[,] newMap = new int[width, height];

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            int neighbours = GetLiveNeighbourCount(x, y);

            // 현재 벽 → 이웃이 적으면 빈 공간으로
            // 현재 빈 공간 → 이웃이 많으면 벽으로
            if (_map[x, y] == 1)
                newMap[x, y] = (neighbours >= survivalThreshold) ? 1 : 0;
            else
                newMap[x, y] = (neighbours >  birthThreshold)    ? 1 : 0;
        }

        _map = newMap;
    }

    /// <summary>3x3 이웃의 살아있는(벽) 셀 수를 반환합니다.</summary>
    private int GetLiveNeighbourCount(int gridX, int gridY)
    {
        int count = 0;

        for (int nx = gridX - 1; nx <= gridX + 1; nx++)
        for (int ny = gridY - 1; ny <= gridY + 1; ny++)
        {
            if (nx == gridX && ny == gridY) continue;

            // 경계 밖은 벽으로 처리
            if (nx < 0 || nx >= width || ny < 0 || ny >= height)
                count++;
            else
                count += _map[nx, ny];
        }
        return count;
    }

    // =========================================================================
    // Gizmos — 맵 시각화
    // =========================================================================

    private void OnDrawGizmos()
    {
        if (_map == null) return;

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Gizmos.color = _map[x, y] == 1 ? Color.black : Color.white;
            Vector3 pos = transform.position
                + new Vector3(x * cellSize, 0f, y * cellSize)
                - new Vector3(width * cellSize / 2f, 0f, height * cellSize / 2f);

            Gizmos.DrawCube(pos, Vector3.one * (cellSize - 0.05f));
        }
    }
}
