using System.Collections;
using Blade.Core.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Player
{
    [Provide]
    public class CheckPoint : MonoBehaviour, IDependencyProvider
    {
        private Player _player;
        private CharacterController _controller;
        [SerializeField] private Transform[] trans;
        [SerializeField] private Image image;
        
        private bool _isActivated = false;
        public int index = 0;

        #region CheckPoint
        private Vector3 _checkpointPosition;

        private const string CheckpointXKey = "CheckpointX";
        private const string CheckpointYKey = "CheckpointY";
        private const string CheckpointZKey = "CheckpointZ";
        
        #endregion

        private void Awake()
        {
            _player = GetComponent<Player>();
            _controller = GetComponent<CharacterController>();
            if (PlayerPrefs.HasKey("index"))
            {
                index = PlayerPrefs.GetInt("index");
            }
            else
            {
                index = 1;
            }
            
            LoadCheckpoint();
            image.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                Cheat(0);
            }
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                Cheat(1);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Cheat(2);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Cheat(3);
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                Cheat(4);
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                Cheat(5);
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                Cheat(6);
            }
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                Cheat(7);
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                Cheat(8);
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                Cheat(9);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                Cheat(10);
            }
#endif
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
            {
                if (hit.gameObject.name == $"CheckPoint{index}" && _isActivated == false)
                {
                    SetCheckpoint(hit.point);
                    CheckPointMethod();
                }
                _isActivated = false;
            }
            if (hit.gameObject.layer == LayerMask.NameToLayer("ResetGround"))
            {
                StartCoroutine(RespawnCoroutine());
            }
            if (hit.gameObject.layer == LayerMask.NameToLayer("Trap"))
            {
                StartCoroutine(RespawnCoroutine());
            }
        }

        private void CheckPointMethod()
        {
            ++index;
            PlayerPrefs.SetInt("index", index);
            PlayerPrefs.Save();
            _isActivated = true;
            image.gameObject.SetActive(true);
            StartCoroutine(WaitCoroutine());
        }
        
        private IEnumerator RespawnCoroutine()
        {
            transform.position = _checkpointPosition; 
            _player.PlayerInput.canJump = true;
            yield return new WaitForSeconds(1f);
        }

        private IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(1f);
            image.gameObject.SetActive(false);
        }
        
        public void SetCheckpoint(Vector3 newCheckpoint)
        {
            _checkpointPosition = newCheckpoint;

            PlayerPrefs.SetFloat(CheckpointXKey, _checkpointPosition.x);
            PlayerPrefs.SetFloat(CheckpointYKey, _checkpointPosition.y);
            PlayerPrefs.SetFloat(CheckpointZKey, _checkpointPosition.z);
            PlayerPrefs.Save();
        }
        
        private void LoadCheckpoint()
        {
            if (PlayerPrefs.HasKey(CheckpointXKey) && PlayerPrefs.HasKey(CheckpointYKey) && PlayerPrefs.HasKey(CheckpointZKey))
            {
                Debug.Log("Have checkpoint");
                float x = PlayerPrefs.GetFloat(CheckpointXKey);
                float y = PlayerPrefs.GetFloat(CheckpointYKey);
                float z = PlayerPrefs.GetFloat(CheckpointZKey);
                _checkpointPosition = new Vector3(x, y, z);
                
                if (_controller != null)
                {
                    _controller.enabled = false;
                    transform.position = _checkpointPosition;
                    _controller.enabled = true; 
                }
            }
            else
            {
                Debug.Log("No checkpoint");
                _checkpointPosition = trans[0].position;
            }
        }
        
        public void ClearCheckpoint()
        {
            PlayerPrefs.DeleteKey(CheckpointXKey);
            PlayerPrefs.DeleteKey(CheckpointYKey);
            PlayerPrefs.DeleteKey(CheckpointZKey);
            PlayerPrefs.DeleteKey("index");
            PlayerPrefs.Save();
    
            Debug.Log("Checkpoint data cleared.");
    
            _checkpointPosition = trans[0].position;
            index = 0;
        }

        private void Cheat(int idx)
        {
            _controller.enabled = false;
            transform.position = trans[idx].position;
            _controller.enabled = true; 
            index = idx;
        }
        
        
    }   
}
