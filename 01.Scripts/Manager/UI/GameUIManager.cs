using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Manager
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private float tweenDuration = 0.3f;

        private bool isTransitioning = false;

        private void Start()
        {
            panel.transform.localScale = Vector3.zero;
            panel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isTransitioning) return;

                if (panel.activeSelf)
                {
                    ClosePanel();
                }
                else
                {
                    OpenPanel();
                }
            }
        }

        public void OpenPanel()
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
            Time.timeScale = 0f;
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

        public void ContinueGame()
        {
            if (!isTransitioning)
            {
                ClosePanel();
            }
        }

        public void TitleScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("TitleScene");
        }
    }
}
