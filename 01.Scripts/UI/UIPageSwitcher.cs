using DG.Tweening;
using UnityEngine;

namespace Code.UI
{
    public class UIPageSwitcher : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private RectTransform mainPanel;
        [SerializeField] private RectTransform soundPanel;

        [Header("Button CanvasGroups")]
        [SerializeField] private CanvasGroup mainButtonGroup;
        [SerializeField] private CanvasGroup soundButtonGroup;

        [Header("Animation Settings")]
        [SerializeField] private float duration = 0.5f;

        private float moveX = 1920f;

        private void Start()
        {
            mainPanel.anchoredPosition = Vector2.zero;
            soundPanel.anchoredPosition = new Vector2(moveX, 0);
            SetButtonAlpha(mainButtonGroup, 1f);
            SetButtonAlpha(soundButtonGroup, 0.5f);
        }

        public void ShowMain()
        {
            Debug.Log("ShowMain");
            mainPanel.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.OutQuart).SetUpdate(true);
            soundPanel.DOAnchorPos(new Vector2(moveX, 0), duration).SetEase(Ease.OutQuart).SetUpdate(true);

            SetButtonAlpha(mainButtonGroup, 1f);
            SetButtonAlpha(soundButtonGroup, 0.5f);
        }

        public void ShowSound()
        {
            Debug.Log("ShowSound");
            mainPanel.DOAnchorPos(new Vector2(-moveX, 0), duration).SetEase(Ease.OutQuart).SetUpdate(true);
            soundPanel.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.OutQuart).SetUpdate(true);

            SetButtonAlpha(mainButtonGroup, 0.5f);
            SetButtonAlpha(soundButtonGroup, 1f);
        }

        private void SetButtonAlpha(CanvasGroup group, float alpha)
        {
            group.DOFade(alpha, 0.3f).SetEase(Ease.OutSine).SetUpdate(true);
        }
        
    }
}