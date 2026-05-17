using Ami.BroAudio;
using DG.Tweening;
using UnityEngine;

namespace Code.UI
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private SoundID sound;

        private void Start()
        {
            BroAudio.Play(sound);
        }

        public void StopMusic()
        {
            BroAudio.Stop(sound);
        }
        
    }
}