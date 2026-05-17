using System.Collections;
using Ami.BroAudio;
using Code.Manager;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Object
{
    public class ObjectClickChecker : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera[] cameras;
        
        [SerializeField] private TitleUIManager titleUI;
        [SerializeField] private TitleSoundUIManager gameUI;
        [SerializeField] private SoundID clickSound;
        [SerializeField] private new string name;

        private bool _isClickable = true;

        private void Awake()
        {
            ResetToFirstCamera();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _isClickable)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == this.transform)
                    {
                        BroAudio.Play(clickSound);

                        SwitchCameraByName(name);
                        
                        if (titleUI != null && name == "Bonfire")
                        {
                            StartCoroutine(ClickRoutine(titleUI.OpenWaitCoroutine())); // 시작
                        }

                        if (titleUI != null && name == "Mine")
                        {
                            StartCoroutine(ClickRoutine(titleUI.OpenPSBWaitCoroutine())); // 외부 링크
                        }

                        if (gameUI != null && name == "Tent")
                        {
                            StartCoroutine(ClickRoutine(titleUI.OpenEndWaitCoroutine())); // 끝내기
                        }

                        if (titleUI != null && name == "Well")
                        {
                            StartCoroutine(ClickRoutine(gameUI.OpenCorutine())); // 설정
                        }
                        
                    }
                }
            }
        }
        
        private void SwitchCameraByName(string objectName)
        {
            int index = -1;

            switch (objectName)
            {
                case "Bonfire": index = 1; break;
                case "Mine":    index = 2; break;
                case "Tent":    index = 3; break;
                case "Well":    index = 4; break;
            }

            if (index >= 0 && index < cameras.Length)
            {
                for (int i = 0; i < cameras.Length; i++)
                    cameras[i].Priority = 0;

                cameras[index].Priority = 10;
            }
        }
        
        public void ResetToFirstCamera()
        {
            for (int i = 0; i < cameras.Length; i++)
                cameras[i].Priority = 0;

            if (cameras.Length > 0)
                cameras[0].Priority = 10;
        }
        
        private IEnumerator ClickRoutine(IEnumerator coroutine)
        {
            _isClickable = false;
            yield return StartCoroutine(coroutine);
            _isClickable = true;
        }
        
    }
}