using Code.Entities;
using UnityEngine;

namespace Code.Player.States
{
    public class PlayerJumpState : PlayerState
    {
        private PlayerJumpCompo _jumpCompo;

        public PlayerJumpState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _animatorTrigger.OnAnimatonEndTrigger += OnJumpEnd;
            _jumpCompo = _player.GetCompo<PlayerJumpCompo>();
            _jumpCompo.Jump();
        }

        public override void Exit()
        {
            base.Exit();
            _animatorTrigger.OnAnimatonEndTrigger -= OnJumpEnd;
        }

        private void OnJumpEnd()
        {
            _player.ChangeState("IDLE");
        }
    }
}