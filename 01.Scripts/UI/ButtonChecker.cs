using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class ButtonChecker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI chooseText; 
        [SerializeField] private Button myButton;
        [SerializeField] private Button mainBtn;
        private TextMeshProUGUI myBtnText;
        private TextMeshProUGUI mainBtnText;

        private void Awake()
        {
            myBtnText = myButton.GetComponentInChildren<TextMeshProUGUI>();
            mainBtnText = mainBtn.GetComponentInChildren<TextMeshProUGUI>();

            chooseText.text = "튜토리얼을 진행하세요!";
            mainBtn.interactable = false;
            mainBtnText.alpha = 0.35f;
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt("TutorialButtonClicked", 0) == 1)
            {
                myButton.interactable = false;
                myBtnText.alpha = 0.35f;
            }
            if (PlayerPrefs.GetInt("MainButtonActive", 0) == 1)
            {
                mainBtn.interactable = true;
                mainBtnText.alpha = 1f;
                chooseText.text = "메인 게임에 들어갈 시간이에요!";
            }
        }
        
        
    }
}