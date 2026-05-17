using Ami.BroAudio;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Object
{
    public class TreeClickChecker : MonoBehaviour
    {
        [SerializeField] private GameObject apple;
        [SerializeField] private TextMeshProUGUI showText;
        [SerializeField] private Transform[] trans;
        [SerializeField] private SoundID clickSound;
        [SerializeField] private float clickValue;
        
        private float _value;
        private int _spawnCnt;
        private bool _isSpawned = false;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == this.transform)
                    {
                        BroAudio.Play(clickSound);
                        _value += clickValue;
                        float remaining = Mathf.Max(0f, 100f - _value);
                        showText.text = $"열매캐기 : {remaining}";
                    }
                }
            }

            if (_value >= 100f && _isSpawned == false && _spawnCnt < 5)
            {
                _spawnCnt++;
                _isSpawned = true;
                int rand = Random.Range(0, trans.Length);
                Instantiate(apple, trans[rand].position, trans[rand].rotation);
                _value = 0f;
                _isSpawned = false;
                showText.text = $"열매캐기 : 100";
            }
            if (_spawnCnt >= 5)
            {
                showText.text = $"열매캐기 : 완료";
            }
        }
        
        
        
        
        
    }
}