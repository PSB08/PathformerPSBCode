using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Manager
{
    public class TitleSoundUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private float tweenDuration = 0.3f;

        private bool isTransitioning = false;

        private void Start()
        {
            panel.transform.localScale = Vector3.zero;
            panel.SetActive(false);
        }

        private void OpenPanel()
        {
            if (panel.activeSelf || isTransitioning) return;
            
            isTransitioning = true;
            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero;
            panel.transform.DOScale(Vector3.one, tweenDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    isTransitioning = false;
                });
        }

        public void ClosePanel()
        {
            if (!panel.activeSelf || isTransitioning) return;
            
            isTransitioning = true;
            panel.transform.DOScale(Vector3.zero, tweenDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    panel.SetActive(false);
                    Time.timeScale = 1f;
                    isTransitioning = false;
                });
        }

        public IEnumerator OpenCorutine()
        {
            yield return new WaitForSeconds(2f);
            OpenPanel();
        }
        
        
    }
}