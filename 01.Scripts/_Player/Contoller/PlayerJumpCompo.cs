using System.Collections;
using Ami.BroAudio;
using Blade.Core.Dependencies;
using Code.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Player
{
    public class PlayerJumpCompo : MonoBehaviour, IEntityComponent
    {
        [Inject] private Player _player;
        [Inject] private CharacterMovement _movement;

        [Header("Jump")]
        [SerializeField] private float jumpHeight = 2.5f;
        [SerializeField] private float gravity = -25f;
        [SerializeField] private float fallMultiplier = 2.2f;
        [SerializeField] private float riseMultiplier = 1.0f;
        [SerializeField] private float lowJumpMultiplier = 2.5f;

        [Header("FX")]
        [SerializeField] private Image jumpImage;
        [SerializeField] private ParticleSystem jumpParticle;
        [SerializeField] private SoundID jumpSound;

        [Header("Cooldown")]
        [SerializeField] private float jumpCooldown = 1.0f;

        private float _jumpTimer;
        private bool _isCoolingDown;
        private bool _jumpHeld;

        private void Awake()
        {
            if (jumpParticle != null)
                jumpParticle.Stop();
        }
        
        public void Initialize(Entity entity)
        {
            
        }

        private void Update()
        {
            HandleGravity();
            UpdateJumpCooldownUI();
        }

        public void SetJumpHeld(bool value)
        {
            _jumpHeld = value;
        }

        public void Jump()
        {
            if (_movement.IsGround == false) return;
            if (_player.PlayerInput.canJump == false) return;

            _player.PlayerInput.canJump = false;
            _isCoolingDown = true;
            _jumpTimer = 0f;
            _jumpHeld = true;

            _movement.VerticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (jumpParticle != null)
                jumpParticle.Play();

            BroAudio.Play(jumpSound);

            StartCoroutine(JumpCooldownCoroutine());
        }

        private IEnumerator JumpCooldownCoroutine()
        {
            yield return new WaitForSeconds(jumpCooldown);
            _player.PlayerInput.canJump = true;
        }

        private void HandleGravity()
        {
            if (_movement.IsGround && _movement.VerticalVelocity < 0f)
            {
                _movement.VerticalVelocity = -2f;
                return;
            }

            float appliedGravity = gravity;

            if (_movement.VerticalVelocity > 0f)
            {
                appliedGravity *= _jumpHeld ? riseMultiplier : lowJumpMultiplier;
            }
            else
            {
                appliedGravity *= fallMultiplier;
            }

            _movement.VerticalVelocity += appliedGravity * Time.deltaTime;
        }

        private void UpdateJumpCooldownUI()
        {
            if (jumpImage == null) return;

            if (!_player.PlayerInput.canJump && !_isCoolingDown)
            {
                jumpImage.fillAmount = 0f;
                return;
            }

            if (_isCoolingDown)
            {
                _jumpTimer += Time.deltaTime;

                float progress = Mathf.Clamp01(_jumpTimer / jumpCooldown);
                jumpImage.fillAmount = progress;

                if (_jumpTimer >= jumpCooldown)
                {
                    _player.PlayerInput.canJump = true;
                    _isCoolingDown = false;
                }
            }
            else
            {
                jumpImage.fillAmount = 1f;
            }
        }
        
    }
}