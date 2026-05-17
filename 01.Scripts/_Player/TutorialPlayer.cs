using System.Collections;
using UnityEngine;

namespace Code.Player
{
    public class TutorialPlayer : MonoBehaviour
    {
        private Player _player;
        private CharacterController _controller;
        [SerializeField] private Transform trans;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
            _controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _player.PlayerInput.canJump = true;
            ResetPlayer();
        }

        private void ResetPlayer()
        {
            _controller.enabled = false;
            transform.position = trans.position;
            _controller.enabled = true; 
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("ResetGround"))
            {
                StartCoroutine(RespawnCoroutine());
            }
            if (hit.gameObject.layer == LayerMask.NameToLayer("Trap"))
            {
                StartCoroutine(RespawnCoroutine());
            }
        }
        
        private IEnumerator RespawnCoroutine()
        {
            transform.position = trans.position; 
            _player.PlayerInput.canJump = true;
            yield return new WaitForSeconds(1f);
        }
        
    }
}