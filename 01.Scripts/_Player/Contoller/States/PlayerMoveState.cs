using Code.Entities;
using UnityEngine;

namespace Code.Player.States
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            base.Update();
            Vector2 movementKey = _player.PlayerInput.MovementKey;
            
            _movement.SetMovementDirection(movementKey);
            if (movementKey.magnitude < _inputThreshold)
                _player.ChangeState("IDLE");
        }

    
    }
}
