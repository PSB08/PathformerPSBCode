using Blade.Core.Dependencies;
using Code.Entities;
using UnityEngine;

namespace Code.Player
{
    [Provide]
    public class CharacterMovement : MonoBehaviour, IEntityComponent, IDependencyProvider
    {
        [Header("Value/Ref")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private Camera mainCamera;
        [field: SerializeField] public float moveSpeed { get; private set; } = 8f;
        [Inject] private Player _player;

        [Header("Boolean")]
        public bool IsGround => controller.isGrounded;

        public bool CanManualMovement { get; set; } = true;
        public bool RestrictZMovement { get; set; } = false;
        public bool IsReturning { get; private set; } = false;

        [Header("Runtime")]
        private Vector3 _autoMovement;
        private Vector3 _velocity;
        private Vector3 _movementDirection;

        public float VerticalVelocity { get; set; }

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void SetMovementDirection(Vector2 movementInput)
        {
            float x = movementInput.x;
            float z = RestrictZMovement ? 0f : movementInput.y;

            Vector3 camForward = mainCamera.transform.forward;
            Vector3 camRight = mainCamera.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 direction = camForward * z + camRight * x;
            _movementDirection = direction.normalized;
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            Move();
        }

        private void CalculateMovement()
        {
            if (CanManualMovement)
            {
                _velocity = _movementDirection * moveSpeed * Time.fixedDeltaTime;
            }
            else
            {
                _velocity = _autoMovement * Time.fixedDeltaTime;
            }

            _velocity.y = VerticalVelocity * Time.fixedDeltaTime;

            Vector3 flatVelocity = new Vector3(_velocity.x, 0f, _velocity.z);
            if (flatVelocity.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(flatVelocity);
                float rotationSpeed = 8f;
                Transform parent = _entity.transform;
                parent.rotation = Quaternion.Lerp(parent.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed);
            }
        }

        private void Move()
        {
            controller.Move(_velocity);

            if (RestrictZMovement)
            {
                Vector3 pos = _player.transform.position;
                _player.transform.position = pos;
            }
        }

        public void StopImmediately()
        {
            _movementDirection = Vector3.zero;
        }
        
    }
}