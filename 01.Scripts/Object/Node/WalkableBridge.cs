using UnityEngine;

namespace Code.Object.NodeMake
{
    public class WalkableBridge : MonoBehaviour
    {
        public float bridgeWidth = 0.5f;

        private LineRenderer _lineRenderer;
        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();
        }

        public void GenerateBridgeMesh()
        {
            Vector3[] points = new Vector3[_lineRenderer.positionCount];
            _lineRenderer.GetPositions(points);

            int segmentCount = points.Length - 1;
            Vector3[] vertices = new Vector3[segmentCount * 8];
            int[] triangles = new int[segmentCount * 12];

            for (int i = 0; i < segmentCount; i++)
            {
                Vector3 forward = (points[i + 1] - points[i]).normalized;
                Vector3 right = Vector3.Cross(Vector3.up, forward) * (bridgeWidth / 2);
                
                vertices[i * 8 + 0] = transform.InverseTransformPoint(points[i] - right);
                vertices[i * 8 + 1] = transform.InverseTransformPoint(points[i] + right);
                vertices[i * 8 + 2] = transform.InverseTransformPoint(points[i + 1] - right);
                vertices[i * 8 + 3] = transform.InverseTransformPoint(points[i + 1] + right);
                
                float thickness = 0.1f;
                Vector3 down = Vector3.down * thickness;

                vertices[i * 8 + 4] = transform.InverseTransformPoint(points[i] - right + down);
                vertices[i * 8 + 5] = transform.InverseTransformPoint(points[i] + right + down);
                vertices[i * 8 + 6] = transform.InverseTransformPoint(points[i + 1] - right + down);
                vertices[i * 8 + 7] = transform.InverseTransformPoint(points[i + 1] + right + down);
                
                triangles[i * 12 + 0] = i * 8 + 0;
                triangles[i * 12 + 1] = i * 8 + 2;
                triangles[i * 12 + 2] = i * 8 + 1;

                triangles[i * 12 + 3] = i * 8 + 1;
                triangles[i * 12 + 4] = i * 8 + 2;
                triangles[i * 12 + 5] = i * 8 + 3;
                
                triangles[i * 12 + 6]  = i * 8 + 5;
                triangles[i * 12 + 7]  = i * 8 + 6;
                triangles[i * 12 + 8]  = i * 8 + 4;

                triangles[i * 12 + 9]  = i * 8 + 5;
                triangles[i * 12 + 10] = i * 8 + 7;
                triangles[i * 12 + 11] = i * 8 + 6;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            _meshFilter.mesh = mesh;
            _meshCollider.sharedMesh = mesh;
        }
        
        
    }
}