using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Object.NodeMake
{
    public class NodeMakeButton : MonoBehaviour
    {
        [SerializeField] private GameObject bridgePrefab;
        private Dictionary<(Node, Node), GameObject> activeBridges = new();
        
        [Header("Nodes")]
        [SerializeField] private Node nodeA;
        [SerializeField] private Node nodeB;
        [SerializeField] private Node nodeC;
        
        [Space(10)]
        [Header("UI")]
        [SerializeField] private Button buttonB;
        [SerializeField] private Button buttonC;
        [SerializeField] private bool isConnected;
        
        [Space(10)]
        [Header("길 생성 가능 범위")]
        [SerializeField] private LayerMask obstacleLayer;          
        [SerializeField] private Vector3 boxSize = new Vector3(1, 1, 1);  
        [SerializeField] private Vector3 boxOffset = Vector3.zero;
        [SerializeField] private Image canMakeRoadImage;
        
        private void Start()
        {
            buttonB.gameObject.SetActive(false);
            buttonC.gameObject.SetActive(false);
            canMakeRoadImage.gameObject.SetActive(false);

            buttonB.onClick.AddListener(() => OnNodeSelected(nodeB));
            buttonC.onClick.AddListener(() => OnNodeSelected(nodeC));
        }
        
        private void Update()
        {
            if (CanEnterMakeRoad())
            {
                canMakeRoadImage.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.R))
                {
                    bool isActive = buttonB.gameObject.activeSelf;
                    buttonB.gameObject.SetActive(!isActive);
                    buttonC.gameObject.SetActive(!isActive);
                }
            }
            else
            {
                canMakeRoadImage.gameObject.SetActive(false);
                buttonB.gameObject.SetActive(false);
                buttonC.gameObject.SetActive(false);
            }
        }
        
        private bool CanEnterMakeRoad()
        {
            Vector3 center = transform.position + transform.rotation * boxOffset;
            return Physics.CheckBox(center, boxSize * 0.5f, transform.rotation, obstacleLayer);
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

        private void OnNodeSelected(Node targetNode)
        {
            var key = (nodeA, targetNode);

            if (nodeA.connectedNodes.Contains(targetNode))
            {
                //삭제
                nodeA.DisconnectFrom(targetNode);

                if (activeBridges.TryGetValue(key, out GameObject bridge))
                {
                    var nodeBridge = bridge.GetComponent<NodePathBridge>();
                    if (nodeBridge != null)
                    {
                        nodeBridge.StartCoroutine(nodeBridge.EraseCurveCoroutine());
                    }
                    else
                    {
                        Destroy(bridge);
                    }
                    activeBridges.Remove(key);
                }
            }
            else
            {
                //생성
                nodeA.ConnectTo(targetNode);

                GameObject bridgeObj = Instantiate(bridgePrefab);
                var bridge = bridgeObj.GetComponent<NodePathBridge>();
                bridge.startPoint = nodeA.transform;
                bridge.endPoint = targetNode.transform;
                bridge.StartCoroutine(bridge.DrawCurveCoroutine());

                activeBridges[key] = bridgeObj;
            }
            buttonB.gameObject.SetActive(false);
            buttonC.gameObject.SetActive(false);
        }
        
    }
}