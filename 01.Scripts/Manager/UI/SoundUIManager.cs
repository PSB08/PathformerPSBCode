using Ami.BroAudio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Manager
{
    public class SoundUIManager : MonoBehaviour
    {
        [Header("Slider")]
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        [Header("Slider Text")]
        [SerializeField] private TextMeshProUGUI masterText;
        [SerializeField] private TextMeshProUGUI musicText;
        [SerializeField] private TextMeshProUGUI sfxText;
        
        [Header("MuteBtn")]
        [SerializeField] private Button masterMuteBtn;
        [SerializeField] private Button musicMuteBtn;
        [SerializeField] private Button sfxMuteBtn;
        
        [Header("Value")]
        [SerializeField] private Sprite sound;
        [SerializeField] private Sprite mute;

        [Header("ButtonSound")] 
        [SerializeField] private SoundID buttonClickSound;

        private bool _isMasterMuted = false;
        private bool _isMusicMuted = false;
        private bool _isSfxMuted = false;

        private void Start()
        {
            LoadVolume();
            LoadMuteStates();
            UpdateButtonIcons();
        
            masterMuteBtn.onClick.AddListener(ToggleMasterMute);
            musicMuteBtn.onClick.AddListener(ToggleMusicMute);
            sfxMuteBtn.onClick.AddListener(ToggleSfxMute);
        }
        
        public void ButtonSound()
        {
            BroAudio.Play(buttonClickSound);
        }
        
        
        public void SetMasterVolume()
        {
            float volume = masterSlider.value;
            BroAudio.SetVolume(BroAudioType.All, volume);
            PlayerPrefs.SetFloat("masterVolume", volume);
            
            masterText.text = $"볼륨 : {(volume * 10f).ToString("0.0")}";

            if (volume == 0.0001f && !_isMasterMuted)
                ToggleMasterMute();
            else if (volume > 0.0001f && _isMasterMuted)
                ToggleMasterMute();
        }
        
        public void SetMusicVolume()
        {
            float volume = musicSlider.value;
            BroAudio.SetVolume(BroAudioType.Music, volume);
            PlayerPrefs.SetFloat("musicVolume", volume);
            
            musicText.text = $"음악 : {(volume * 10f).ToString("0.0")}";

            if (volume == 0.0001f && !_isMusicMuted)
                ToggleMusicMute();
            else if (volume > 0.0001f && _isMusicMuted)
                ToggleMusicMute();
        }

        public void SetSfxVolume()
        {
            float volume = sfxSlider.value;
            BroAudio.SetVolume(BroAudioType.SFX, volume);
            PlayerPrefs.SetFloat("sfxVolume", volume);
            
            sfxText.text = $"효과음 : {(volume * 10f).ToString("0.0")}";

            if (volume == 0.0001f && !_isSfxMuted)
                ToggleSfxMute();
            else if (volume > 0.0001f && _isSfxMuted)
                ToggleSfxMute();
        }
        
        private void LoadVolume()
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 1f);
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);

            SetMasterVolume();
            SetMusicVolume();
            SetSfxVolume();
        }


        private void ToggleMasterMute()
        {
            _isMasterMuted = !_isMasterMuted;
            float volume = _isMasterMuted ? 0f : masterSlider.value;
            BroAudio.SetVolume(BroAudioType.All, volume);
            SaveMuteState("masterMute", _isMasterMuted);
            UpdateButtonIcons();
        }
        
        private void ToggleMusicMute()
        {
            _isMusicMuted = !_isMusicMuted;
            float volume = _isMusicMuted ? 0f : musicSlider.value;
            BroAudio.SetVolume(BroAudioType.Music, volume);
            SaveMuteState("musicMute", _isMusicMuted);
            UpdateButtonIcons();
        }

        private void ToggleSfxMute()
        {
            _isSfxMuted = !_isSfxMuted;
            float volume = _isSfxMuted ? 0f : sfxSlider.value;
            BroAudio.SetVolume(BroAudioType.SFX, volume);
            SaveMuteState("sfxMute", _isSfxMuted);
            UpdateButtonIcons();
        }
        
        private void SaveMuteState(string key, bool state)
        {
            PlayerPrefs.SetInt(key, state ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadMuteStates()
        {
            _isMasterMuted = PlayerPrefs.GetInt("masterMute", 0) == 1;
            _isMusicMuted = PlayerPrefs.GetInt("musicMute", 0) == 1;
            _isSfxMuted = PlayerPrefs.GetInt("sfxMute", 0) == 1;

            BroAudio.SetVolume(BroAudioType.All, _isMasterMuted ? 0f : masterSlider.value);
            BroAudio.SetVolume(BroAudioType.Music, _isMusicMuted ? 0f : musicSlider.value);
            BroAudio.SetVolume(BroAudioType.SFX, _isSfxMuted ? 0f : sfxSlider.value);
        }

        private void UpdateButtonIcons()
        {
            masterMuteBtn.GetComponentInChildren<Image>().sprite = _isMasterMuted ? mute : sound;
            musicMuteBtn.GetComponentInChildren<Image>().sprite = _isMusicMuted ? mute : sound;
            sfxMuteBtn.GetComponentInChildren<Image>().sprite = _isSfxMuted ? mute : sound;
        }
        
    }
}