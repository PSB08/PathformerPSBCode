using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Object
{
    public class CamChangeChecker : MonoBehaviour
    {
        [Header("전환 차단 설정 (Box 기반)")]
        [SerializeField] private LayerMask obstacleLayer;          
        [SerializeField] private Vector3 boxSize = new Vector3(1, 1, 1);  
        [SerializeField] private Vector3 boxOffset = Vector3.zero;

        [SerializeField] private CinemachineCamera virtualCamera;
        [SerializeField] private CinemachineCamera secondaryVirtualCamera;
        [SerializeField] private TextMeshProUGUI warningText;

        private void Start()
        {
            warningText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (CanSwitchView())
            {
                if (virtualCamera.gameObject.activeSelf && virtualCamera.Priority == 1)
                {
                    warningText.text = "카메라 각도가 플레이에 영향을 줄 수 있습니다. 변환 가능한 구역으로 이동하세요.";
                    warningText.gameObject.SetActive(true);
                }
                else
                {
                    warningText.gameObject.SetActive(false);
                }
            }
            else
            {
                warningText.gameObject.SetActive(false);
            }
        }

        private bool CanSwitchView()
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