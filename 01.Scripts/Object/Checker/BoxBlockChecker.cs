using UnityEngine;

namespace Code.Object
{
    public class BoxBlockChecker : MonoBehaviour, ICanBlockViewSwitch
    {
        [Header("전환 차단 설정 (Box 기반)")]
        [SerializeField] private LayerMask obstacleLayer;          
        [SerializeField] private Vector3 boxSize = new Vector3(1, 1, 1);  
        [SerializeField] private Vector3 boxOffset = Vector3.zero;
        
        public bool CanSwitchView()
        {
            Vector3 center = transform.position + transform.rotation * boxOffset;
            return !Physics.CheckBox(center, boxSize * 0.5f, transform.rotation, obstacleLayer);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

            Vector3 center = transform.position + transform.rotation * boxOffset;
            Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxSize);

            Gizmos.matrix = Matrix4x4.identity;
        }
#endif
        
        
    }
}