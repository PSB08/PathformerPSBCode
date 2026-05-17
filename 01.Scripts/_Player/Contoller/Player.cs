using System;
using System.Collections;
using Blade.Core.Dependencies;
using Code.Axis;
using Code.Entities;
using Code.FSM;
using Code.Object;
using TMPro;
using UnityEngine;


namespace Code.Player
{
    [Provide]
    public class Player : Entity, IDependencyProvider
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [SerializeField] private StateDataSO[] states;

        [field : SerializeField] public Action OnGameEnd;
        [field : SerializeField] public Action OnTutorialEnd;

        public EntityStateMachine _stateMachine;
        [SerializeField] private RotateAxisScript rotateAxis;
        [SerializeField] private TextMeshProUGUI stateTxt;
        private int? lastLayerHit = null;
        
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);
            PlayerInput.OnJumpPressed += HandleJumpKeyPressed;
            stateTxt.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerInput.OnJumpPressed -= HandleJumpKeyPressed;
        }
        
        private void HandleJumpKeyPressed()
        {
            ChangeState("JUMP");
        }

        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }

        public void ChangeState(string newStateName)
        {
            if (_stateMachine.CurrentStateName == newStateName) return;
            _stateMachine.ChangeState(newStateName);
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            int currentLayer = hit.gameObject.layer;

            // 이전과 같은 Layer면 아무 작업 안 함
            if (lastLayerHit == currentLayer)
                return;
            
            lastLayerHit = currentLayer;
            
            var teleport = hit.gameObject.GetComponent<ViewSwitchTeleport>();
            
            if (currentLayer == LayerMask.NameToLayer("GoalObj"))
            {
                OnGameEnd?.Invoke();
            }
            if (currentLayer == LayerMask.NameToLayer("TutorialGoalObj"))
            {
                OnTutorialEnd?.Invoke();
            }
            if (currentLayer == LayerMask.NameToLayer("CanJumpArea"))
            {
                stateTxt.text = "비가 그쳐 플레이어가 점프할 수 있습니다.";
                PlayerInput.canJump = true;
                StartCoroutine(CoolDownCoroutine());
            }
            if (currentLayer == LayerMask.NameToLayer("CannotJumpArea"))
            {
                stateTxt.color = Color.red;
                stateTxt.text = "비가 내려 플레이어가 점프할 수 없습니다.";
                PlayerInput.canJump = false;
                StartCoroutine(CoolDownCoroutine());
            }
            if (currentLayer == LayerMask.NameToLayer("CamChange"))
            {
                stateTxt.text = "안개가 자욱해 플레이어가 잠시 방향 감각을 잃었습니다.";
                rotateAxis.ToggleCameraOnceWithCooldown();
                StartCoroutine(CoolDownCoroutine());
            }
            if (teleport != null)
            {
                teleport.Teleport(gameObject);
            }
        }

        private IEnumerator CoolDownCoroutine()
        {
            stateTxt.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            stateTxt.gameObject.SetActive(false);
            stateTxt.color = Color.white;
        }
        
    }
}


