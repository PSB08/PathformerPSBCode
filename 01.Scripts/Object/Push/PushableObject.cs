using UnityEngine;
using UnityEngine.UI;

namespace Code.Object
{
    public class PushableObject : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private Vector3 detectionBoxSize = new Vector3(3f, 3f, 3f);
        [SerializeField] private LayerMask playerLayer;
        
        [Space(10)]
        [Header("Push Settings")]
        [SerializeField] private Vector2 maxPushRange = new Vector2(5f, 5f);
        [SerializeField] private float pushDistance = 1.5f;
        [SerializeField] private float pushSpeed = 3f;
        [SerializeField] private float pushCooldown = 1.0f; 
        private bool _isPushCooldown = false;
        private float _pushTimer = 0f;

        [Space(10)]
        [Header("UI Settings")]
        [SerializeField] private Image uiPrompt;

        [SerializeField] private KeyCode key = KeyCode.E;

        [SerializeField] private bool pushableIn3D;

        private bool _canPushInCurrentMode = true;
        
        private Vector3 _startPosition;
        private Transform _player;
        private bool _isPushed;
        private Vector3 _targetPosition;

        private void Awake()
        {
            _startPosition = transform.position;
            SetUiPrompt(false);
        }

        private void Update()
        {
            UpdatePushCooldownUI();
            DetectPlayer();

            if (_isPushed)
                MoveTowardsTarget();
        }

        private void DetectPlayer()
        {
            Collider[] hits = Physics.OverlapBox(transform.position, detectionBoxSize * 0.5f, Quaternion.identity, playerLayer);

            bool playerInRange = hits.Length > 0;
            
            if (playerInRange && _canPushInCurrentMode)
            {
                if (_player == null)
                    _player = hits[0].transform;

                if (CanPushInCurrentDirection())
                {
                    SetUiPrompt(true);

                    if (Input.GetKeyDown(key) && !_isPushed && !_isPushCooldown)
                    {
                        StartPush();
                    }
                }
                else
                {
                    SetUiPrompt(false);
                }
            }
            else
            {
                _player = null;
                SetUiPrompt(false);
            }
        }
        
        public void SetViewMode(bool is3DMode)
        {
            _canPushInCurrentMode = is3DMode == pushableIn3D;
            if (!_canPushInCurrentMode)
                SetUiPrompt(false);
        }
        
        private void UpdatePushCooldownUI()
        {
            if (uiPrompt == null || !_isPushCooldown) return;

            _pushTimer += Time.deltaTime;
            float progress = _pushTimer / pushCooldown;
            uiPrompt.fillAmount = Mathf.Clamp01(progress);

            if (_pushTimer >= pushCooldown)
            {
                _isPushCooldown = false;
                uiPrompt.fillAmount = 1f;
            }
        }

        private void StartPush()
        {
            Vector3 pushDirection = GetPushDirection();
            Vector3 proposedTarget = transform.position + pushDirection * pushDistance;
            _targetPosition = ClampTargetPosition(proposedTarget);
            _isPushed = true;
            
            _isPushCooldown = true;
            _pushTimer = 0f;
            if (uiPrompt != null)
            {
                uiPrompt.fillAmount = 0f;
                uiPrompt.gameObject.SetActive(true);
            }
        }

        private Vector3 GetPushDirection()
        {
            Vector3 forward = _player.forward;
            return Mathf.Abs(forward.x) > Mathf.Abs(forward.z)
                ? new Vector3(Mathf.Sign(forward.x), 0, 0)
                : new Vector3(0, 0, Mathf.Sign(forward.z));
        }

        private Vector3 ClampTargetPosition(Vector3 proposedTarget)
        {
            Vector3 offset = proposedTarget - _startPosition;

            float clampedX = Mathf.Clamp(offset.x, -maxPushRange.x, maxPushRange.x);
            float clampedZ = Mathf.Clamp(offset.z, -maxPushRange.y, maxPushRange.y);

            return _startPosition + new Vector3(clampedX, 0, clampedZ);
        }
        
        private bool CanPushInCurrentDirection()
        {
            if (_player == null) return false;

            Vector3 pushDirection = GetPushDirection();
            Vector3 proposedTarget = transform.position + pushDirection * pushDistance;
            Vector3 clampedTarget = ClampTargetPosition(proposedTarget);

            // 실제 밀려고 하는 방향으로 클램핑된 결과가 동일하면, 밀 수 없는 거리임
            return Vector3.Distance(transform.position, clampedTarget) > 0.01f;
        }

        private void MoveTowardsTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, pushSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
                _isPushed = false;
        }

        private void SetUiPrompt(bool active)
        {
            if (uiPrompt != null)
                uiPrompt.gameObject.SetActive(active);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, detectionBoxSize);
        }
        
        
    }
}