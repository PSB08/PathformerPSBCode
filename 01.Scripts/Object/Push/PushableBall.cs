using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Object
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class PushableBall : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private Vector3 detectionBoxSize = new Vector3(3f, 3f, 3f);
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private LayerMask objectLayer;
        [SerializeField] private float destroyTime = 10f;

        [Space(10)]
        [Header("Push Settings")]
        [SerializeField] private float pushForce = 10f;

        [Space(10)]
        [Header("UI Settings")]
        [SerializeField] private Image uiPrompt;

        [SerializeField] private KeyCode key = KeyCode.E;

        private Transform _player;
        private bool _hasPushed = false;
        private Rigidbody _rb;
        private SphereCollider _collider;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<SphereCollider>();
            _rb.useGravity = true;
            SetUiPrompt(false);
        }

        private void Update()
        {
            DetectPlayer();

            if (!_hasPushed && _player != null && Input.GetKeyDown(key))
            {
                PushBall();
            }
        }

        private void DetectPlayer()
        {
            Collider[] hits = Physics.OverlapBox(transform.position, detectionBoxSize * 0.5f, Quaternion.identity, playerLayer);

            if (hits.Length > 0)
            {
                if (_player == null)
                    _player = hits[0].transform;

                SetUiPrompt(true);
            }
            else
            {
                _player = null;
                SetUiPrompt(false);
            }
        }

        private void PushBall()
        {
            _collider.excludeLayers = playerLayer;
            Vector3 pushDir = GetPushDirection();
            _rb.AddForce(pushDir * pushForce, ForceMode.VelocityChange);
            _hasPushed = true;
        }

        private Vector3 GetPushDirection()
        {
            Vector3 direction = _player.forward;
            direction.y = 0f;
            direction.Normalize();
            
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                direction = new Vector3(Mathf.Sign(direction.x), 0f, 0f);
            }
            else
            {
                direction = new Vector3(0f, 0f, Mathf.Sign(direction.z));
            }

            return direction;
        }

        private void SetUiPrompt(bool active)
        {
            if (uiPrompt != null)
                uiPrompt.gameObject.SetActive(active);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (_hasPushed && ((1 << collision.gameObject.layer) & objectLayer) != 0)
            {
                _collider.excludeLayers = default;
                _rb.angularVelocity = Vector3.zero;
                Destroy(gameObject, destroyTime);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, detectionBoxSize);
        }
        
    }
}

