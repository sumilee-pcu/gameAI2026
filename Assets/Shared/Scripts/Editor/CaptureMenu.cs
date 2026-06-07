#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace GameAI.CaptureTools
{
    /// <summary>
    /// 챕터 씬을 열고 Play 모드로 들어가 자동 캡쳐(AutoCapture)를 트리거하는 메뉴.
    /// 메뉴 한 번 클릭 → 씬 열림 → Play → Captures/ 에 PNG 저장 → Play 종료.
    /// </summary>
    public static class CaptureMenu
    {
        private const string Lab04Scene = "Assets/Labs/Lab04_UtilityAI/Scenes/Lab04_UtilityAI.unity";
        private const string Lab05Scene = "Assets/Labs/Lab05_RandomProbability/Scenes/Lab05_RandomProbability.unity";

        [MenuItem("GameAI/Capture/Lab04 UtilityAI 캡쳐")]
        public static void CaptureLab04() => OpenAndPlay(Lab04Scene);

        [MenuItem("GameAI/Capture/Lab05 RandomProbability 캡쳐")]
        public static void CaptureLab05() => OpenAndPlay(Lab05Scene);

        private static void OpenAndPlay(string scenePath)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }
            EditorSceneManager.OpenScene(scenePath);
            EditorApplication.EnterPlaymode();
        }
    }
}
#endif
