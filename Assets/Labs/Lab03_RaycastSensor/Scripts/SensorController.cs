using UnityEngine;

/// <summary>
/// Lab03: 센서 컨트롤러 — VisionSensor 결과에 반응합니다.
/// </summary>
[RequireComponent(typeof(VisionSensor))]
public class SensorController : MonoBehaviour
{
    // =========================================================================
    // 인스펙터 필드
    // =========================================================================

    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color alertColor   = Color.red;
    [SerializeField] private bool  logDetection = true;

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private VisionSensor _sensor;
    private Renderer     _renderer;
    private bool         _wasAlerted;

    // =========================================================================
    // 프로퍼티
    // =========================================================================

    public bool IsAlerted => _sensor.HasVisibleTarget;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Awake()
    {
        _sensor   = GetComponent<VisionSensor>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        SetColor(defaultColor);
    }

    private void Update()
    {
        bool isAlerted = _sensor.HasVisibleTarget;

        // 감지 시작
        if (isAlerted && !_wasAlerted)
        {
            SetColor(alertColor);
            if (logDetection)
                Debug.Log($"[SensorController] 타겟 감지! ({_sensor.VisibleTargetCount}개)");
            // TODO: 경고 UI 표시, 사운드 재생, Chase 상태 전환 등
        }

        // 감지 소실
        if (!isAlerted && _wasAlerted)
        {
            SetColor(defaultColor);
            if (logDetection)
                Debug.Log("[SensorController] 타겟 시야에서 소실.");
        }

        _wasAlerted = isAlerted;
    }

    // =========================================================================
    // 내부 메서드
    // =========================================================================

    private void SetColor(Color color)
    {
        if (_renderer != null)
            _renderer.material.color = color;
    }
}
