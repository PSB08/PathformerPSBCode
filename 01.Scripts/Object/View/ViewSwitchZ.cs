using UnityEngine;
using DG.Tweening;

namespace Code.Object
{
    public class ViewSwitchZ : MonoBehaviour
    {
        [Tooltip("3D 모드에서 z 고정인가?")] public bool fixedIn3D;
        public float zFix;
        private float _originalZ;
        
        private void Awake()
        {
            _originalZ = transform.position.z;
        }

        public void SetZFix(bool is3DMode)
        {
            fixedIn3D = is3DMode;

            float targetZ = is3DMode ? _originalZ : zFix;

            transform.DOMoveZ(targetZ, 0.5f).SetEase(Ease.InOutSine);
        }
        
        
    }
}