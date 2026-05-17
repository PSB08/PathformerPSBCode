using Blade.Core.Dependencies;
using Code.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Manager
{
    public class UICntManager : MonoBehaviour
    {
        [Inject] private Player.Player _player;
        [SerializeField] private CheckPoint checkPoint;
        [SerializeField] private Image uiPrompt;
        [SerializeField] private Button restartBtn;
        
        private void Start()
        { 
            Time.timeScale = 1;
            uiPrompt.gameObject.SetActive(false);
            restartBtn.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _player.OnGameEnd += GameEndMethod;
            _player.OnTutorialEnd += TutorialEndMethod;
        }

        private void OnDisable()
        {
            _player.OnGameEnd -= GameEndMethod;
            _player.OnTutorialEnd -= TutorialEndMethod;
        }

        private void TutorialEndMethod()
        {
            _player.PlayerInput.canJump = false;
            uiPrompt.gameObject.SetActive(true);
            Time.timeScale = 0;
            restartBtn.gameObject.SetActive(true);
        }
        
        private void GameEndMethod()
        {
            if (checkPoint == null) return;
            
            _player.PlayerInput.canJump = false;
            checkPoint.ClearCheckpoint();
            uiPrompt.gameObject.SetActive(true);
            Time.timeScale = 0;
            restartBtn.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            restartBtn.gameObject.SetActive(false);
            uiPrompt.gameObject.SetActive(false);
            _player.PlayerInput.canJump = true;
            SceneManager.LoadScene("TitleScene");
        }
        
        
    }    
}

