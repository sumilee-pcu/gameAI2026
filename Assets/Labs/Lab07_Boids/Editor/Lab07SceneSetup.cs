#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAI.Lab07
{
    public static class Lab07SceneSetup
    {
        private const string LabRoot       = "Assets/Labs/Lab07_Boids";
        private const string ScenesFolder  = LabRoot + "/Scenes";
        private const string PrefabsFolder = LabRoot + "/Prefabs";
        private const string AssetsFolder  = LabRoot + "/Settings";
        private const string SceneName     = "Lab07_Boids";
        private const string PrefabName    = "BoidPrefab";
        private const string SettingsName  = "DefaultBoidSettings";

        [MenuItem("GameAI/Lab07 Boids/Setup Scene")]
        public static void SetupScene()
        {
            EnsureFolder(PrefabsFolder);
            EnsureFolder(AssetsFolder);

            BoidSettings settings = CreateOrLoadSettings();
            GameObject prefab     = CreateOrLoadBoidPrefab();
            Scene scene           = CreateScene();

            GameObject managerGo = new GameObject("BoidManager");
            Undo.RegisterCreatedObjectUndo(managerGo, "Create BoidManager");

            BoidManager manager = managerGo.AddComponent<BoidManager>();

            SerializedObject so = new SerializedObject(manager);
            so.FindProperty("settings").objectReferenceValue   = settings;
            so.FindProperty("boidPrefab").objectReferenceValue = prefab.GetComponent<Boid>();
            so.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.MarkSceneDirty(scene);

            string scenePath = ScenesFolder + "/" + SceneName + ".unity";
            EditorSceneManager.SaveScene(scene, scenePath);

            Debug.Log("[Lab07] Scene setup complete: " + scenePath);
            EditorUtility.DisplayDialog(
                "Lab07 Boids",
                "씬 세팅이 완료되었습니다.\n\n" +
                "- 씬: " + scenePath + "\n" +
                "- 프리팹: " + PrefabsFolder + "/" + PrefabName + ".prefab\n" +
                "- 설정: " + AssetsFolder + "/" + SettingsName + ".asset\n\n" +
                "Play 버튼을 눌러 군집 이동을 확인하세요.",
                "확인");
        }

        private static BoidSettings CreateOrLoadSettings()
        {
            string path = AssetsFolder + "/" + SettingsName + ".asset";
            BoidSettings existing = AssetDatabase.LoadAssetAtPath<BoidSettings>(path);
            if (existing != null) return existing;

            BoidSettings settings = ScriptableObject.CreateInstance<BoidSettings>();
            AssetDatabase.CreateAsset(settings, path);
            AssetDatabase.SaveAssets();
            return settings;
        }

        private static GameObject CreateOrLoadBoidPrefab()
        {
            string path = PrefabsFolder + "/" + PrefabName + ".prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing != null) return existing;

            GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            tmp.name = PrefabName;
            tmp.transform.localScale = new Vector3(0.3f, 0.3f, 0.6f);

            CapsuleCollider col = tmp.GetComponent<CapsuleCollider>();
            if (col != null) Object.DestroyImmediate(col);

            tmp.AddComponent<Boid>();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(tmp, path);
            Object.DestroyImmediate(tmp);
            return prefab;
        }

        private static Scene CreateScene()
        {
            EnsureFolder(ScenesFolder);
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            GameObject cam = GameObject.Find("Main Camera");
            if (cam != null)
                cam.transform.SetPositionAndRotation(new Vector3(0f, 25f, -30f), Quaternion.Euler(35f, 0f, 0f));
            return scene;
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
