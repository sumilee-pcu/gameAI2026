using System.Collections;
using System.IO;
using UnityEngine;

/// <summary>
/// Play 모드 진입 후 일정 시간이 지나면 Game 화면을 PNG로 저장한다.
/// 저장 위치: &lt;프로젝트 루트&gt;/Captures/{fileName}
/// 챕터별 실행 장면을 GitHub용으로 캡쳐하기 위한 공용 컴포넌트.
/// </summary>
public class AutoCapture : MonoBehaviour
{
    [SerializeField] private string fileName = "capture.png";
    [SerializeField] private float  delay    = 3f;   // 장면이 안정될 시간
    [SerializeField] private int    superSize = 1;   // 2면 2배 해상도
    [SerializeField] private bool   exitPlayAfter = true;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);

        string dir = Path.Combine(Directory.GetCurrentDirectory(), "Captures");
        Directory.CreateDirectory(dir);
        string path = Path.Combine(dir, fileName);

        ScreenCapture.CaptureScreenshot(path, Mathf.Max(1, superSize));
        Debug.Log("[AutoCapture] 저장됨: " + path);

        // 파일이 디스크에 기록될 시간을 준다
        yield return new WaitForSeconds(0.8f);

#if UNITY_EDITOR
        if (exitPlayAfter)
            UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
