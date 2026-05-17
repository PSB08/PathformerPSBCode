using UnityEngine;

namespace Code.Object
{
    public class ViewSwitchTeleport : MonoBehaviour
    {
        [Tooltip("이 오브젝트가 반응할 시점 (true = 3D, false = 2D)")]
        public bool is3DMode;
        
        [SerializeField] private Transform trans2D;
        [SerializeField] private Transform trans3D;

        public void Teleport(GameObject target)
        {
            var controller = target.GetComponent<CharacterController>();
            if (controller == null) return;

            if (is3DMode)
                controller.enabled = false;
            target.transform.position = is3DMode ? trans3D.position : trans2D.position;
            if (is3DMode)
                controller.enabled = true;
        }

        public void SetViewMode(bool is3d)
        {
            is3DMode = is3d;
        }

    }    
}

