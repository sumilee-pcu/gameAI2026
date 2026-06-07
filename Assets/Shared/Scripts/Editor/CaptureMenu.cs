#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GameAI.CaptureTools
{
    /// <summary>
    /// 챕터 씬을 열고 Play 모드로 들어가 자동 캡쳐(AutoCapture)를 트리거하는 메뉴.
    /// 메뉴 한 번 클릭 → 씬 열림 → Play → Captures/ 에 PNG 저장 → Play 종료.
    /// 씬에 AutoCapture가 없으면 임시로 주입하며, 씬 파일은 저장하지 않아 원본이 보존된다.
    /// </summary>
    public static class CaptureMenu
    {
        [MenuItem("GameAI/Capture/Lab04 UtilityAI 캡쳐")]
        public static void CaptureLab04() =>
            OpenEnsureAndPlay("Assets/Labs/Lab04_UtilityAI/Scenes/Lab04_UtilityAI.unity", "Lab04_UtilityAI.png", 4f);

        [MenuItem("GameAI/Capture/Lab05 RandomProbability 캡쳐")]
        public static void CaptureLab05() =>
            OpenEnsureAndPlay("Assets/Labs/Lab05_RandomProbability/Scenes/Lab05_RandomProbability.unity", "Lab05_RandomProbability.png", 6f);

        [MenuItem("GameAI/Capture/Lab07 Boids(Flocking) 캡쳐")]
        public static void CaptureLab07() =>
            OpenEnsureAndPlay("Assets/Labs/Lab07_Boids/Scenes/Lab07_Boids.unity", "Lab07_Boids_Flocking.png", 5f);

        private static void OpenEnsureAndPlay(string scenePath, string pngName, float delay)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            EditorSceneManager.OpenScene(scenePath);

            // 씬에 AutoCapture가 없으면 임시 오브젝트로 주입(저장하지 않음 → 원본 씬 불변)
            AutoCapture ac = Object.FindFirstObjectByType<AutoCapture>();
            if (ac == null)
            {
                GameObject go = new GameObject("AutoCapture (temp)");
                ac = go.AddComponent<AutoCapture>();
            }

            SerializedObject so = new SerializedObject(ac);
            so.FindProperty("fileName").stringValue = pngName;
            so.FindProperty("delay").floatValue = delay;
            so.ApplyModifiedPropertiesWithoutUndo();

            EditorApplication.EnterPlaymode();
        }
    }
}
#endif
