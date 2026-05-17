using System.Collections.Generic;
using UnityEngine;

namespace Code.Object
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class CombineMesh : MonoBehaviour
    {
        public GameObject[] go;
        public Vector3 offset = Vector3.zero;

        private void Start()
        {
            if (go == null || go.Length == 0)
            {
                Debug.LogWarning("CombineMesh: GameObject array 'go' is null or empty.");
                return;
            }

            List<CombineInstance> combineList = new List<CombineInstance>();

            for (int i = 0; i < go.Length; i++)
            {
                if (go[i] == null)
                {
                    Debug.LogWarning($"CombineMesh: GameObject at index {i} is null. Skipping.");
                    continue;
                }

                MeshFilter mf = go[i].GetComponent<MeshFilter>();
                if (mf == null || mf.sharedMesh == null)
                {
                    Debug.LogWarning($"CombineMesh: MeshFilter or sharedMesh missing on GameObject '{go[i].name}'. Skipping.");
                    continue;
                }

                Matrix4x4 worldToLocal = transform.worldToLocalMatrix;
                Matrix4x4 meshMatrix = mf.transform.localToWorldMatrix;
                Matrix4x4 finalMatrix = worldToLocal * meshMatrix * Matrix4x4.Translate(offset);

                CombineInstance ci = new CombineInstance
                {
                    mesh = mf.sharedMesh,
                    transform = finalMatrix
                };

                combineList.Add(ci);
            }

            if (combineList.Count == 0)
            {
                Debug.LogWarning("CombineMesh: No valid meshes found to combine.");
                return;
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.name = "CombinedMesh_Runtime";
            combinedMesh.CombineMeshes(combineList.ToArray(), true, true);

            // 메시 적용
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh = combinedMesh;

            // 메시 콜라이더도 추가
            MeshCollider meshCollider = GetComponent<MeshCollider>() ?? gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = combinedMesh;
            meshCollider.convex = true;
        }
        
        
        
    }
}
