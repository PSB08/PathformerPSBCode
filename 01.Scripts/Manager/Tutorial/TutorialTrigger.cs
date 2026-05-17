using UnityEngine;

namespace Code.Manager
{
    public class TutorialTrigger : MonoBehaviour
    {
        [TextArea]
        [SerializeField] private string tutoText;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TutorialManager.Instance.ShowTutorial(tutoText);
            }
        }
        
        
    }
}