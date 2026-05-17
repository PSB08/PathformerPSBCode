using System;
using UnityEngine;

namespace Code.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimatonEndTrigger;
        public Action<bool> OnRollingStatusChange;
        public Action OnAttackVFXTrigger;

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        private void AnimationEnd()
        {
            OnAnimatonEndTrigger?.Invoke();
        }

        private void RollingStart() => OnRollingStatusChange?.Invoke(true);
        private void RollingEnd() => OnRollingStatusChange?.Invoke(false);
        private void PlayAttackVFX() => OnAttackVFXTrigger?.Invoke();
    }
}