using UnityEngine;

/// <summary>
/// Lab12: 펄린 노이즈 기반 지형 생성기.
/// Chapter 10 PerlinTexture.cs 기반, Unity 6 Terrain API 업데이트.
/// </summary>
[RequireComponent(typeof(Terrain))]
public class TerrainGenerator : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("지형 크기")]
    [SerializeField] private int   width  = 256;
    [SerializeField] private int   height = 256;

    [Header("노이즈 파라미터")]
    [SerializeField] private float scale       = 20f;
    [SerializeField] private int   octaves     = 4;
    [SerializeField] private float persistence = 0.5f;   // 진폭 감소율
    [SerializeField] private float lacunarity  = 2f;     // 주파수 증가율

    [Header("시드")]
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    // =========================================================================
    // 내부 참조
    // =========================================================================

    private Terrain _terrain;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Awake()
    {
        _terrain = GetComponent<Terrain>();
    }

    private void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
        GenerateTerrain();
    }

    // =========================================================================
    // 지형 생성
    // =========================================================================

    [ContextMenu("Regenerate Terrain")]
    public void GenerateTerrain()
    {
        _terrain = GetComponent<Terrain>();
        _terrain.terrainData = GenerateTerrainData(_terrain.terrainData);
    }

    private TerrainData GenerateTerrainData(TerrainData data)
    {
        data.heightmapResolution = width + 1;
        data.size = new Vector3(width, 50f, height);
        data.SetHeights(0, 0, GenerateHeights());
        return data;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            float sampleX = (float)x / width  * scale + offsetX;
            float sampleY = (float)y / height * scale + offsetY;
            heights[x, y] = CalculateHeight(sampleX, sampleY);
        }
        return heights;
    }

    /// <summary>
    /// 멀티-옥타브 펄린 노이즈로 높이 계산.
    /// 옥타브(Octaves)를 더할수록 디테일이 추가됨.
    /// </summary>
    private float CalculateHeight(float x, float y)
    {
        float amplitude  = 1f;
        float frequency  = 1f;
        float noiseValue = 0f;
        float maxAmplitude = 0f;

        for (int i = 0; i < octaves; i++)
        {
            noiseValue   += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;
            maxAmplitude += amplitude;

            amplitude  *= persistence;  // 진폭 감소
            frequency  *= lacunarity;   // 주파수 증가
        }

        return noiseValue / maxAmplitude; // 0~1 정규화
    }

    // =========================================================================
    // Gizmos
    // =========================================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            transform.position + new Vector3(width / 2f, 25f, height / 2f),
            new Vector3(width, 50f, height)
        );
    }
}
