#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace GameAI.Lab04
{
    /// <summary>
    /// Lab04 Utility AI 데모 씬을 자동 구성한다.
    /// 메뉴: GameAI/Lab04 UtilityAI/Setup Scene
    /// 에이전트(캡슐) + 플레이어 타깃(노란 구) + 카메라 + 자동 캡쳐를 배치하고 씬을 저장한다.
    /// </summary>
    public static class Lab04SceneSetup
    {
        private const string LabRoot      = "Assets/Labs/Lab04_UtilityAI";
        private const string ScenesFolder = LabRoot + "/Scenes";
        private const string SceneName    = "Lab04_UtilityAI";

        [MenuItem("GameAI/Lab04 UtilityAI/Setup Scene")]
        public static void SetupScene()
        {
            EnsureFolder(ScenesFolder);

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // 카메라
            GameObject cam = GameObject.Find("Main Camera");
            if (cam != null)
                cam.transform.SetPositionAndRotation(new Vector3(0f, 17f, -15f), Quaternion.Euler(45f, 0f, 0f));

            // 바닥
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(3f, 1f, 3f);
            ground.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.22f, 0.24f, 0.28f));

            // 에이전트 (캡슐)
            GameObject agent = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            agent.name = "UtilityAgent";
            agent.transform.position = new Vector3(0f, 1f, 0f);
            agent.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.7f, 0.7f, 0.7f));
            UtilityAgent ua = agent.AddComponent<UtilityAgent>();

            // 플레이어 타깃 (노란 구)
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            player.name = "Player";
            player.tag  = "Player";
            player.transform.position   = new Vector3(6f, 0.6f, 0f);
            player.transform.localScale = Vector3.one * 1.2f;
            player.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(1f, 0.85f, 0.2f));
            Lab04PlayerMover mover = player.AddComponent<Lab04PlayerMover>();

            // 참조 연결
            SerializedObject soAgent = new SerializedObject(ua);
            soAgent.FindProperty("player").objectReferenceValue = player.transform;
            soAgent.ApplyModifiedPropertiesWithoutUndo();

            SerializedObject soMover = new SerializedObject(mover);
            soMover.FindProperty("center").objectReferenceValue = agent.transform;
            soMover.ApplyModifiedPropertiesWithoutUndo();

            // 자동 캡쳐
            GameObject cap = new GameObject("AutoCapture");
            AutoCapture ac = cap.AddComponent<AutoCapture>();
            SerializedObject soCap = new SerializedObject(ac);
            soCap.FindProperty("fileName").stringValue = SceneName + ".png";
            soCap.FindProperty("delay").floatValue     = 4f;
            soCap.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.MarkSceneDirty(scene);
            string scenePath = ScenesFolder + "/" + SceneName + ".unity";
            EditorSceneManager.SaveScene(scene, scenePath);

            Debug.Log("[Lab04] Scene setup complete: " + scenePath);
            EditorUtility.DisplayDialog("Lab04 Utility AI",
                "씬 세팅 완료:\n" + scenePath +
                "\n\nPlay ▶ 를 누르면 행동 점수가 표시되고 4초 후 Captures/ 에 캡쳐됩니다." +
                "\n(메뉴 GameAI/Capture/Lab04 UtilityAI 캡쳐 로 자동 실행 가능)", "확인");
        }

        private static Material MakeMaterial(Color c)
        {
            Shader sh = GraphicsSettings.defaultRenderPipeline != null
                ? Shader.Find("Universal Render Pipeline/Lit")
                : Shader.Find("Standard");
            if (sh == null) sh = Shader.Find("Standard");
            Material m = new Material(sh);
            m.color = c;
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
            return m;
        }

        private static void EnsureFolder(string folder)
        {
            if (AssetDatabase.IsValidFolder(folder)) return;
            string parent = Path.GetDirectoryName(folder).Replace('\\', '/');
            string leaf   = Path.GetFileName(folder);
            if (!AssetDatabase.IsValidFolder(parent)) EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, leaf);
        }
    }
}
#endif
