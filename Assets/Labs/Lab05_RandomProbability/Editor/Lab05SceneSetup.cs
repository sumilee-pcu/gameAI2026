#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace GameAI.Lab05
{
    /// <summary>
    /// Lab05 Random/Probability 데모 씬을 자동 구성한다.
    /// 메뉴: GameAI/Lab05 RandomProbability/Setup Scene
    /// </summary>
    public static class Lab05SceneSetup
    {
        private const string LabRoot      = "Assets/Labs/Lab05_RandomProbability";
        private const string ScenesFolder = LabRoot + "/Scenes";
        private const string SceneName    = "Lab05_RandomProbability";

        [MenuItem("GameAI/Lab05 RandomProbability/Setup Scene")]
        public static void SetupScene()
        {
            EnsureFolder(ScenesFolder);

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            GameObject cam = GameObject.Find("Main Camera");
            if (cam != null)
                cam.transform.SetPositionAndRotation(new Vector3(0f, 5f, -8f), Quaternion.Euler(25f, 0f, 0f));

            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(2f, 1f, 2f);
            ground.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.22f, 0.24f, 0.28f));

            GameObject agent = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            agent.name = "ProbabilityAgent";
            agent.transform.position = new Vector3(0f, 1f, 0f);
            agent.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.30f, 0.65f, 1f));
            agent.AddComponent<ProbabilityAgent>();

            GameObject cap = new GameObject("AutoCapture");
            AutoCapture ac = cap.AddComponent<AutoCapture>();
            SerializedObject soCap = new SerializedObject(ac);
            soCap.FindProperty("fileName").stringValue = SceneName + ".png";
            soCap.FindProperty("delay").floatValue     = 6f; // 분포가 어느 정도 수렴할 시간
            soCap.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.MarkSceneDirty(scene);
            string scenePath = ScenesFolder + "/" + SceneName + ".unity";
            EditorSceneManager.SaveScene(scene, scenePath);

            Debug.Log("[Lab05] Scene setup complete: " + scenePath);
            EditorUtility.DisplayDialog("Lab05 Random/Probability",
                "씬 세팅 완료:\n" + scenePath +
                "\n\nPlay ▶ 를 누르면 가중치 대비 실제 분포가 표시되고 6초 후 Captures/ 에 캡쳐됩니다.", "확인");
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
