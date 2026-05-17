using System;
using UnityEngine;

namespace Code.Effect
{
    public class FogLightingEffect : MonoBehaviour
    {
        [Header("전환 차단 설정 (Box 기반)")] [SerializeField]
        private LayerMask obstacleLayer;

        [SerializeField] private Vector3 boxSize = new Vector3(1, 1, 1);
        [SerializeField] private Vector3 boxOffset = Vector3.zero;

        [Header("Fog Setting")] [SerializeField]
        private float originValue;

        [SerializeField] private float startValue;
        [SerializeField] private Color originColor;
        [SerializeField] private Color startColor;

        private void Update()
        {
            if (CanSwitchView())
            {
                RenderSettings.fogStartDistance = startValue;
                RenderSettings.fogColor = startColor;
            }
            else
            {
                RenderSettings.fogStartDistance = originValue;
                RenderSettings.fogColor = originColor;
            }
        }

        public bool CanSwitchView()
        {
            Vector3 center = transform.position + transform.rotation * boxOffset;
            return Physics.CheckBox(center, boxSize * 0.5f, transform.rotation, obstacleLayer);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Vector3 center = transform.position + transform.rotation * boxOffset;
            Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxSize);

            Gizmos.matrix = Matrix4x4.identity;
        }
#endif



    }
}