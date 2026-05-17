using Blade.Core.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Effect
{
    [Provide]
    public class IrisOutTransition : MonoBehaviour, IDependencyProvider
    {
        private Animator _animator;
        private Image _image;
        private readonly int _circleSizeId = Shader.PropertyToID("_CircleSize");
        [SerializeField] private bool isIn = true;

        public float circleSize = 0;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _image = GetComponent<Image>();
            AnimateOut();
        }

        private void Update()
        {
            _image.materialForRendering.SetFloat(_circleSizeId, circleSize);
        }

        public void AnimateIn()
        {
            if (isIn) return;
            
            _animator.SetTrigger("In");
            isIn = true;
        }

        private void AnimateOut()
        {
            if (!isIn) return;
            
            _animator.SetTrigger("Out");
            isIn = false;
        }
        
        
    }
}
