using Ami.BroAudio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI
{
    public class UIButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float targetScale = 1.2f;
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private Ease easeType = Ease.OutBack;
        [SerializeField] private SoundID pointerEnterSound;

        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            BroAudio.Play(pointerEnterSound);
            transform.DOScale(_originalScale * targetScale, duration).SetEase(easeType).SetUpdate(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(_originalScale, duration).SetEase(easeType).SetUpdate(true);
        }
        
    }
}