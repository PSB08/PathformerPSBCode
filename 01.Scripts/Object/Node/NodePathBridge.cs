using System.Collections;
using UnityEngine;

namespace Code.Object.NodeMake
{
    public class NodePathBridge : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;
        [SerializeField] private int curveResolution = 20;
        [SerializeField] private float arcHeight = 2.0f;
        [SerializeField] private float drawDelay = 0.02f;

        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 0;
            _lineRenderer.useWorldSpace = true;
        }

        public IEnumerator DrawCurveCoroutine()
        {
            Vector3[] curvePoints = new Vector3[curveResolution];
            for (int i = 0; i < curveResolution; i++)
            {
                float t = i / (float)(curveResolution - 1);
                curvePoints[i] = GetArcPoint(t);
            }

            for (int i = 0; i < curveResolution; i++)
            {
                _lineRenderer.positionCount = i + 1;
                _lineRenderer.SetPosition(i, curvePoints[i]);

                WalkableBridge bridgeMesh = GetComponent<WalkableBridge>();
                if (bridgeMesh != null)
                {
                    bridgeMesh.GenerateBridgeMesh();
                }
                yield return new WaitForSeconds(drawDelay);
            }
        }
        
        public IEnumerator EraseCurveCoroutine()
        {
            int count = _lineRenderer.positionCount;

            for (int i = count - 1; i >= 1; i--)
            {
                _lineRenderer.positionCount = i;

                WalkableBridge bridgeMesh = GetComponent<WalkableBridge>();
                if (bridgeMesh != null)
                {
                    bridgeMesh.GenerateBridgeMesh();
                }
                yield return new WaitForSeconds(drawDelay);
            }
            Destroy(gameObject);
        }

        private Vector3 GetArcPoint(float t)
        {
            Vector3 mid = (startPoint.position + endPoint.position) / 2 + Vector3.up * arcHeight;
            Vector3 p0 = Vector3.Lerp(startPoint.position, mid, t);
            Vector3 p1 = Vector3.Lerp(mid, endPoint.position, t);
            return Vector3.Lerp(p0, p1, t);
        }
        
        
    }
}