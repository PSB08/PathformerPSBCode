using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Manager
{
    public class CamChangeManager : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera[] cameras;
        [SerializeField] private float switchCooldown = 1f;
        [SerializeField] private KeyCode key = KeyCode.C;
        
        private int _currentIdx = 0;
        private float _lastSwitchTime = -Mathf.Infinity;


        private void Start()
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].gameObject.SetActive(i == _currentIdx);
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(key) && Time.time - _lastSwitchTime >= switchCooldown)
            {
                cameras[_currentIdx].gameObject.SetActive(false);
                _currentIdx = (_currentIdx + 1) % cameras.Length;
                cameras[_currentIdx].gameObject.SetActive(true);

                _lastSwitchTime = Time.time;
            }
        }
        
        
        
    }
}