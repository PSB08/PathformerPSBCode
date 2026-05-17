using Code.Entities;
using UnityEngine;

namespace Code.Player.States
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
        }

        public override void Update()
        {
            base.Update();
            Vector2 movementKey = _player.PlayerInput.MovementKey;
            
            _movement.SetMovementDirection(movementKey);
            if (movementKey.magnitude > _inputThreshold || _movement.IsReturning)
                _player.ChangeState("MOVE");
        }
        
    }
}
