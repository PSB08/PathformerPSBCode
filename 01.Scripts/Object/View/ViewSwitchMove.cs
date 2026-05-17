using DG.Tweening;
using UnityEngine;

namespace Code.Object
{
    public class ViewSwitchMove : MonoBehaviour
    {
        [Tooltip("3D 모드에서 움직이는가?")] public bool moveable3D;
        [Tooltip("움직일 x 목표 위치")] public float moveX;
        [Tooltip("움직일 y 목표 위치")] public float moveY;
        [Tooltip("움직일 z 목표 위치")] public float moveZ;

        [Tooltip("움직이는 시간")] public float moveDuration;

        private Vector3 _originPos;
        private Tween _moveTween;

        private void Awake()
        {
            _originPos = transform.position;
        }

        public void SetViewMove(bool is3dMode)
        {
            bool shouldMove = is3dMode == moveable3D;

            _moveTween?.Kill();

            if (shouldMove)
            {
                Vector3 offset = new Vector3(moveX, moveY, moveZ);
                Vector3 start = _originPos;
                Vector3 end = _originPos + offset;

                Vector3 currentPos = transform.position;
            
                float totalDistance = Vector3.Distance(start, end);
                float progress = Vector3.Dot(currentPos - start, (end - start).normalized) / totalDistance;
                progress = Mathf.Clamp01(progress);
            
                float remainingDuration = moveDuration * (1f - progress);
            
                _moveTween = transform.DOMove(end, remainingDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        _moveTween = transform.DOMove(start, moveDuration)
                            .SetEase(Ease.Linear)
                            .SetLoops(-1, LoopType.Yoyo);
                    });
            }
        }
        
        
    }    
}

