using System;
using System.Collections;
using System.Linq;
using Ami.BroAudio;
using Code.Object;
using Code.Player;
using UnityEngine;
using Unity.Cinemachine;
using Blade.Core.Dependencies;
using DG.Tweening;
using UnityEngine.UI;

namespace Code.Axis
{
    public class RotateAxisScript : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineCamera virtualCamera;
		[SerializeField] private CinemachineCamera secondaryVirtualCamera;
        private CinemachineBasicMultiChannelPerlin _perlin;
        private CinemachineBasicMultiChannelPerlin _secondaryPerlin;

        [Header("2D/3D 전환 설정")]
        [SerializeField] private KeyCode toggleKey = KeyCode.F;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float transitionDuration = 0.5f;
        [SerializeField] private Image canSwitchImage;
        [SerializeField] private SoundID successSound;
        [SerializeField] private SoundID errorSound;
        [SerializeField] private float switchCooldown = 1.0f;
        [SerializeField] private float orthographicSize2D = 10f;
        
        [Header("캠 전환 설정")]
        [SerializeField] private float cameraSwitchCooldown = 3f;
        [SerializeField] private Image screenOverlayImage;
        private bool _canSwitchCamera = true;
        private float _lastCamSwitchTime = -Mathf.Infinity;
        
        
        private float _lastSwitchTime = -Mathf.Infinity;
        private float _switchTimer = 0f;
        private bool _isSwitchCooldown = false;
        private bool _isPlayingCooldownFeedback = false;
        private bool _isPrimaryCamera = true;
        
        
        
        #region Error Mat
        
        [Header("오류 메테리얼 설정")]
        [SerializeField] private SkinnedMeshRenderer head;
        
        [SerializeField] private Material headOriginMaterial;
        [SerializeField] private Material errorMat;
        
        #endregion
        
        #region View
        private ViewSwitchShow[] _viewShowObjects;
        private ViewSwitchZ[] _viewZObjects;
        private ViewSwitchCollider[] _viewColliders;
        private ViewSwitchMove[] _viewMoves;
        private PushableObject[] _pushableObjects;
        private ViewSwitchTeleport[] _teleports;
        private ICanBlockViewSwitch[] _viewBlockers;
        private bool _is3DMode = true;
        #endregion
        
        [Inject] private CharacterMovement _characterMovement;

        [Obsolete("Obsolete")]
        private void Awake()
        {
            var allMonoBehaviours = FindObjectsOfType<MonoBehaviour>();
            
            _viewShowObjects = allMonoBehaviours.OfType<ViewSwitchShow>().ToArray();
            _viewZObjects = allMonoBehaviours.OfType<ViewSwitchZ>().ToArray();
            _viewColliders = allMonoBehaviours.OfType<ViewSwitchCollider>().ToArray();
            _viewMoves = allMonoBehaviours.OfType<ViewSwitchMove>().ToArray();
            _pushableObjects = allMonoBehaviours.OfType<PushableObject>().ToArray();
            _teleports = allMonoBehaviours.OfType<ViewSwitchTeleport>().ToArray();
            _viewBlockers = allMonoBehaviours.OfType<ICanBlockViewSwitch>().ToArray();
        }

        private void Start()
        {
            _isPrimaryCamera = PlayerPrefs.GetInt("IsPrimaryCamera", 1) == 1;
            
            _perlin = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            _secondaryPerlin = secondaryVirtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            
            virtualCamera.gameObject.SetActive(_isPrimaryCamera);
            secondaryVirtualCamera.gameObject.SetActive(!_isPrimaryCamera);
            
            UpdateView(true);
        }

        private void Update()
        {
            UpdateSwitchCooldownUI();

        #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.C))
            {
                ToggleCameraOnceWithCooldown();
            }
        #endif
            
            if (_isPlayingCooldownFeedback) return;
            
            if (Input.GetKeyDown(toggleKey))
            {
                TryToggleViewMode();
            }
            if (!_canSwitchCamera && Time.time - _lastCamSwitchTime >= cameraSwitchCooldown)
            {
                _canSwitchCamera = true;
            }
        }

        private void TryToggleViewMode()
        {
            if (Time.time - _lastSwitchTime < switchCooldown)
            {
                Debug.Log("전환 쿨타임 대기 중...");
                StartCoroutine(PlayCooldownFeedback());
                return;
            }

            if (!CanSwitchView())
            {
                Debug.Log("전환 실패: 전환을 막는 오브젝트가 있음.");
                StartCoroutine(PlayBlockerFeedback());
                return;
            }

            BroAudio.Play(successSound);
            _is3DMode = !_is3DMode;
            _lastSwitchTime = Time.time;
            
            _isSwitchCooldown = true;
            _switchTimer = 0f;
            canSwitchImage.fillAmount = 0f;
            canSwitchImage.enabled = true;
            
            UpdateView(false);
        }
        
        private void ToggleCamera()
        {
            _isPrimaryCamera = !_isPrimaryCamera;

            virtualCamera.gameObject.SetActive(_isPrimaryCamera);
            secondaryVirtualCamera.gameObject.SetActive(!_isPrimaryCamera);
            
            PlayerPrefs.SetInt("IsPrimaryCamera", _isPrimaryCamera ? 1 : 0);
            PlayerPrefs.Save();

            UpdateView(false);
        }

        public void ToggleCameraOnceWithCooldown()
        {
            if (!_canSwitchCamera && Time.time - _lastCamSwitchTime < cameraSwitchCooldown)
                return;

            _canSwitchCamera = false;
            _lastCamSwitchTime = Time.time;

            ToggleCamera();
        }
        
        private void UpdateSwitchCooldownUI()
        {
            if (canSwitchImage == null) return;

            if (_isSwitchCooldown)
            {
                _switchTimer += Time.deltaTime;

                float progress = _switchTimer / switchCooldown;
                canSwitchImage.fillAmount = Mathf.Clamp01(progress);

                if (_switchTimer >= switchCooldown)
                {
                    _isSwitchCooldown = false;
                    canSwitchImage.enabled = false;
                }
            }
            else
            {
                if (CanSwitchView())
                {
                    canSwitchImage.fillAmount = 1f;
                    canSwitchImage.enabled = true;
                }
                else
                {
                    canSwitchImage.enabled = false;
                }
            }
        }

        private bool CanSwitchView()
        {
            foreach (var blocker in _viewBlockers)
            {
                if (!blocker.CanSwitchView())
                {
                    return false;
                }
            }
            return true;
        }

        private void UpdateView(bool instant)
        {
            mainCamera.orthographic = !_is3DMode;

            if (!_is3DMode)
            {
                virtualCamera.Lens.OrthographicSize = orthographicSize2D;
                secondaryVirtualCamera.Lens.OrthographicSize = orthographicSize2D;
            }

            Vector3 targetCamPos = _is3DMode
                ? new Vector3(0, 5, -10)
                : new Vector3(0, 0, -10);

            Quaternion targetCamRot;

            if (_isPrimaryCamera)
            {
                targetCamRot = _is3DMode
                    ? Quaternion.Euler(25, 0, 0)
                    : Quaternion.Euler(0, 0, 0);
            }
            else
            {
                targetCamRot = _is3DMode
                    ? Quaternion.Euler(25, -90, 0)
                    : Quaternion.Euler(0, -90, 0);
            }

            float duration = instant ? 0f : transitionDuration;

            mainCamera.transform.DOMove(targetCamPos, duration).SetEase(Ease.InOutSine);

            if (_isPrimaryCamera)
            {
                virtualCamera.transform.DORotateQuaternion(targetCamRot, duration).SetEase(Ease.InOutSine);
            }
            else
            {
                secondaryVirtualCamera.transform.DORotateQuaternion(targetCamRot, duration).SetEase(Ease.InOutSine);
            }

            SwitchingMode();

            if (_characterMovement != null)
            {
                _characterMovement.RestrictZMovement = !_is3DMode;
            }
        }

        private void SwitchingMode()
        {
            foreach (var showable in _viewShowObjects)
            {
                showable.SetShowMode(_is3DMode);
            }

            foreach (var zSwitchable in _viewZObjects)
            {
                zSwitchable.SetZFix(_is3DMode);
            }

            foreach (var switchCollider in _viewColliders)
            {
                switchCollider.SetViewMode(_is3DMode);
            }

            foreach (var pushableObject in _pushableObjects)
            {
                pushableObject.SetViewMode(_is3DMode);
            }

            foreach (var movableObject in _viewMoves)
            {
                movableObject.SetViewMove(_is3DMode);
            }
            
            foreach (var teleport in _teleports)
            {
                teleport.SetViewMode(_is3DMode);
            }
        }
        
        private IEnumerator PlayCooldownFeedback()
        {
            if (_isPlayingCooldownFeedback) yield break;
            
            _isPlayingCooldownFeedback = true;
            
            if (canSwitchImage != null)
            {
                Color originalColor = canSwitchImage.color;
                BroAudio.Play(errorSound);
                canSwitchImage.DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                
                RectTransform rect = canSwitchImage.rectTransform;
                Vector3 originalPos = rect.localPosition;
                rect.DOShakePosition(0.3f, new Vector3(10f, 0, 0), 10, 90, false, true)
                    .OnComplete(() => rect.localPosition = originalPos);

                yield return new WaitForSeconds(1f);
                canSwitchImage.color = originalColor;
            }
            _isPlayingCooldownFeedback = false;
        }
        
        private IEnumerator PlayBlockerFeedback()
        {
            if (_perlin != null && _secondaryPerlin != null)
            {
                _perlin.AmplitudeGain = 2f;
                _perlin.FrequencyGain = 3f;
                _secondaryPerlin.AmplitudeGain = 2f;
                _secondaryPerlin.FrequencyGain = 3f;
            }

            BroAudio.Play(errorSound);
            if (screenOverlayImage != null)
            {
                screenOverlayImage.color = new Color(1f, 0f, 0f, 0f);
                screenOverlayImage.DOFade(0.4f, 0.1f)
                    .OnComplete(() => screenOverlayImage.DOFade(0f, 0.5f));
            }

            head.material = errorMat;
            yield return new WaitForSeconds(1f);
            head.material = headOriginMaterial;

            if (_perlin != null && _secondaryPerlin != null)
            {
                _perlin.AmplitudeGain = 0f;
                _perlin.FrequencyGain = 0f;
                _secondaryPerlin.AmplitudeGain = 0f;
                _secondaryPerlin.FrequencyGain = 0f;
            }
            BroAudio.Stop(errorSound);
        }
        
        
        
    }
}
