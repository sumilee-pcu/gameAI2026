using UnityEngine;

/// <summary>
/// Lab11: ML-Agents RollerAgent — 스텁(Stub) 버전
/// ============================================================
/// ML-Agents 패키지가 설치되지 않아 임시 MonoBehaviour로 대체됩니다.
///
/// [ML-Agents 설치 방법]
/// Window > Package Manager > + > Add package from git URL:
///   https://github.com/Unity-Technologies/ml-agents.git?path=com.unity.ml-agents#release_21
///
/// 설치 완료 후 Assets/Labs/Lab11_MLAgents/Scripts/RollerAgent_Full.cs.txt 내용을
/// 이 파일(RollerAgent.cs)에 복사·붙여넣기 하세요.
/// ============================================================
/// </summary>
public class RollerAgent : MonoBehaviour
{
    [Header("⚠ ML-Agents 패키지 미설치")]
    [TextArea(4, 6)]
    public string installGuide =
        "Window > Package Manager > + >\n" +
        "Add package from git URL >\n" +
        "https://github.com/Unity-Technologies/ml-agents.git" +
        "?path=com.unity.ml-agents#release_21";

    void Start()
    {
        Debug.LogWarning(
            "[Lab11] ML-Agents 패키지가 설치되지 않았습니다.\n" +
            "Package Manager > Add from git URL 로 설치 후\n" +
            "RollerAgent_Full.cs.txt 내용으로 이 스크립트를 교체하세요."
        );
    }
}
