using UnityEngine;

/// <summary>
/// Lab12: 펄린 노이즈 텍스처 시각화.
/// Plane에 부착하면 노이즈 패턴을 실시간으로 확인할 수 있습니다.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class NoiseVisualizer : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [Header("텍스처 설정")]
    [SerializeField] private int   textureWidth  = 256;
    [SerializeField] private int   textureHeight = 256;
    [SerializeField] private float scale         = 20f;

    [Header("바이옴 모드")]
    [SerializeField] private bool useBiomeColors = false;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Start()
    {
        GenerateTexture();
    }

    // =========================================================================
    // 텍스처 생성
    // =========================================================================

    [ContextMenu("Regenerate Texture")]
    public void GenerateTexture()
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        Color[] pixels = new Color[textureWidth * textureHeight];

        float offsetX = Random.Range(0f, 9999f);
        float offsetY = Random.Range(0f, 9999f);

        for (int y = 0; y < textureHeight; y++)
        for (int x = 0; x < textureWidth; x++)
        {
            float xCoord = (float)x / textureWidth  * scale + offsetX;
            float yCoord = (float)y / textureHeight * scale + offsetY;
            float noise  = Mathf.PerlinNoise(xCoord, yCoord);

            pixels[y * textureWidth + x] = useBiomeColors
                ? GetBiomeColor(noise)
                : new Color(noise, noise, noise);
        }

        texture.SetPixels(pixels);
        texture.Apply();

        GetComponent<Renderer>().material.mainTexture = texture;
    }

    /// <summary>높이값에 따라 바이옴 색상을 반환합니다.</summary>
    private Color GetBiomeColor(float height)
    {
        if (height < 0.2f) return new Color(0.1f, 0.3f, 0.8f);  // 깊은 바다
        if (height < 0.3f) return new Color(0.2f, 0.5f, 0.9f);  // 바다
        if (height < 0.4f) return new Color(0.9f, 0.85f, 0.6f); // 모래사장
        if (height < 0.6f) return new Color(0.2f, 0.7f, 0.2f);  // 초원
        if (height < 0.7f) return new Color(0.3f, 0.5f, 0.2f);  // 숲
        if (height < 0.85f) return new Color(0.5f, 0.4f, 0.3f); // 산
        return Color.white;                                       // 눈 덮인 산
    }
}
