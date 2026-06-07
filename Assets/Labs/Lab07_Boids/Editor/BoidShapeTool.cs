#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameAI.Lab07
{
    /// <summary>
    /// Boid 프리팹의 외형을 캡슐 → 새/화살(콘) 모양으로 바꾼다.
    /// Boid 는 진행 방향으로 transform.forward(+Z)를 향하므로,
    /// 끝(tip)이 +Z를 가리키는 납작한 콘 메시를 생성해 군집이 방향성을 갖도록 한다.
    /// 메뉴: GameAI/Lab07 Boids/Make Boid Bird-shaped
    /// </summary>
    public static class BoidShapeTool
    {
        private const string LabRoot     = "Assets/Labs/Lab07_Boids";
        private const string MeshFolder  = LabRoot + "/Meshes";
        private const string PrefabPath  = LabRoot + "/Prefabs/BoidPrefab.prefab";
        private const string MeshPath    = MeshFolder + "/BoidBird.asset";
        private const string MatPath     = MeshFolder + "/BoidBird.mat";

        [MenuItem("GameAI/Lab07 Boids/Make Boid Bird-shaped")]
        public static void MakeBirdShaped()
        {
            EnsureFolder(MeshFolder);

            // 1) 메시 생성/저장
            Mesh mesh = BuildBirdMesh();
            Mesh existingMesh = AssetDatabase.LoadAssetAtPath<Mesh>(MeshPath);
            if (existingMesh != null)
            {
                existingMesh.Clear();
                existingMesh.vertices  = mesh.vertices;
                existingMesh.triangles = mesh.triangles;
                existingMesh.RecalculateNormals();
                existingMesh.RecalculateBounds();
                EditorUtility.SetDirty(existingMesh);
                mesh = existingMesh;
            }
            else
            {
                AssetDatabase.CreateAsset(mesh, MeshPath);
            }

            // 2) 양면 렌더 머티리얼 (실루엣이 항상 보이도록 Cull Off)
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(MatPath);
            if (mat == null)
            {
                Shader sh = GraphicsSettings.defaultRenderPipeline != null
                    ? Shader.Find("Universal Render Pipeline/Lit")
                    : Shader.Find("Standard");
                if (sh == null) sh = Shader.Find("Standard");
                mat = new Material(sh) { name = "BoidBird" };
                AssetDatabase.CreateAsset(mat, MatPath);
            }
            Color c = new Color(0.85f, 0.92f, 1f);
            mat.color = c;
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", c);
            if (mat.HasProperty("_Cull")) mat.SetFloat("_Cull", 0f); // Off = 양면
            mat.doubleSidedGI = true;
            EditorUtility.SetDirty(mat);

            AssetDatabase.SaveAssets();

            // 3) 프리팹에 메시/머티리얼 적용 + 콜라이더 제거 + 스케일 정리
            GameObject root = PrefabUtility.LoadPrefabContents(PrefabPath);
            try
            {
                MeshFilter mf = root.GetComponent<MeshFilter>();
                if (mf == null) mf = root.AddComponent<MeshFilter>();
                mf.sharedMesh = mesh;

                MeshRenderer mr = root.GetComponent<MeshRenderer>();
                if (mr == null) mr = root.AddComponent<MeshRenderer>();
                mr.sharedMaterial = mat;

                Collider col = root.GetComponent<Collider>();
                if (col != null) Object.DestroyImmediate(col, true);

                root.transform.localScale = Vector3.one;

                PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }

            AssetDatabase.Refresh();
            Debug.Log("[Lab07] Boid 외형을 새/콘 모양으로 변경했습니다: " + MeshPath);
        }

        /// <summary>+Z 끝이 뾰족한 납작한 콘(새/화살) 메시.</summary>
        private static Mesh BuildBirdMesh()
        {
            const int seg = 14;
            const float tipZ  = 0.6f;   // 앞쪽 꼭짓점
            const float baseZ = -0.4f;  // 뒤쪽 바닥
            const float rx = 0.24f;     // 좌우 폭(날개)
            const float ry = 0.10f;     // 상하 두께

            var verts = new List<Vector3>();
            var tris  = new List<int>();

            int tip = verts.Count;        verts.Add(new Vector3(0f, 0f, tipZ));
            int baseCenter = verts.Count; verts.Add(new Vector3(0f, 0f, baseZ));

            int ringStart = verts.Count;
            for (int i = 0; i < seg; i++)
            {
                float a = (float)i / seg * Mathf.PI * 2f;
                verts.Add(new Vector3(Mathf.Cos(a) * rx, Mathf.Sin(a) * ry, baseZ));
            }

            for (int i = 0; i < seg; i++)
            {
                int a = ringStart + i;
                int b = ringStart + (i + 1) % seg;
                // 옆면 (tip → ring)
                tris.Add(tip); tris.Add(a); tris.Add(b);
                // 바닥 캡
                tris.Add(baseCenter); tris.Add(b); tris.Add(a);
            }

            Mesh m = new Mesh { name = "BoidBird" };
            m.SetVertices(verts);
            m.SetTriangles(tris, 0);
            m.RecalculateNormals();
            m.RecalculateBounds();
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
