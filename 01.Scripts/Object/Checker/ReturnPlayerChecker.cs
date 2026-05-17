using Blade.Core.Dependencies;
using UnityEngine;

namespace Code.Object
{
    [Provide]
    public class ReturnPlayerChecker : MonoBehaviour, IDependencyProvider
    {
        [Header("맵뚫 차단 설정 (Box 기반)")]
        public LayerMask obstacleLayer;          
        public Vector3 boxSize = new Vector3(1, 1, 1);  
        public Vector3 boxOffset = Vector3.zero;
        public Transform returnTrans;
        
        public bool CanReturnPlayer()
        {
            Vector3 center = transform.position + transform.rotation * boxOffset;
            return Physics.CheckBox(center, boxSize * 0.5f, transform.rotation, obstacleLayer);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Vector3 center = transform.position + transform.rotation * boxOffset;
            Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxSize);

            Gizmos.matrix = Matrix4x4.identity;
        }
#endif

    }
}