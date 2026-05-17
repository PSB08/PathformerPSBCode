using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Code.Manager
{
    public class TitleUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject psbPanel;
        [SerializeField] private float tweenDuration = 0.3f;

        private bool _isTransitioning = false;
        private bool _isPsbTransition = false;
        
        private void Awake()
        {
            Time.timeScale = 1f;
            panel.SetActive(false);
            psbPanel.SetActive(false);
        }

        private void Update()
        {
            //#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                PlayerPrefs.DeleteKey("TutorialButtonClicked");
                PlayerPrefs.DeleteKey("MainButtonActive");
                PlayerPrefs.Save();
            }
            //#endif
        }

        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void StartTutorial()
        {
            PlayerPrefs.SetInt("TutorialButtonClicked", 1);
            PlayerPrefs.SetInt("MainButtonActive", 1);
            PlayerPrefs.Save();
            
            SceneManager.LoadScene("TutorialScene");
        }
        

        public void ExitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
        private void OpenPanel()
        {
            if (panel.activeSelf || _isTransitioning) return;
            
            _isTransitioning = true;
            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero;
            panel.transform.DOScale(Vector3.one, tweenDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _isTransitioning = false;
                });
        }

        public void ClosePanel()
        {
            if (!panel.activeSelf || _isTransitioning) return;
            
            _isTransitioning = true;
            panel.transform.DOScale(Vector3.zero, tweenDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    panel.SetActive(false);
                    Time.timeScale = 1f;
                    _isTransitioning = false;
                });
        }
        
        
        private void OpenPSBPanel()
        {
            if (psbPanel.activeSelf || _isPsbTransition) return;
            
            _isPsbTransition = true;
            psbPanel.SetActive(true);
            psbPanel.transform.localScale = Vector3.zero;
            psbPanel.transform.DOScale(Vector3.one, tweenDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _isPsbTransition = false;
                });
        }
        
        public void ClosePSBPanel()
        {
            if (!psbPanel.activeSelf || _isPsbTransition) return;
            
            _isPsbTransition = true;
            psbPanel.transform.DOScale(Vector3.zero, tweenDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    psbPanel.SetActive(false);
                    Time.timeScale = 1f;
                    _isPsbTransition = false;
                });
        }

        public IEnumerator OpenWaitCoroutine()
        {
            yield return new WaitForSeconds(2f);
            OpenPanel();
        }

        public IEnumerator OpenEndWaitCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ExitGame();
        }

        public IEnumerator OpenPSBWaitCoroutine()
        {
            yield return new WaitForSeconds(2f);
            OpenPSBPanel();
        }
        
    }
}
