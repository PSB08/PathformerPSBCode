using System.Collections;
using TMPro;
using UnityEngine;

namespace Code.Manager
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }

        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private TextMeshProUGUI tutorialText;

        private Coroutine currentCoroutine;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            tutorialPanel.SetActive(false);
            PlayerPrefs.DeleteKey("IsPrimaryCamera");
        }

        public void ShowTutorial(string text, float duration = 2f)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            currentCoroutine = StartCoroutine(ShowCoroutine(text, duration));
        }

        private IEnumerator ShowCoroutine(string text, float duration)
        {
            tutorialPanel.SetActive(true);
            tutorialText.text = text;

            yield return new WaitForSeconds(duration);

            tutorialPanel.SetActive(false);
        }
        
        
    }
}