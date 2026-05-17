using UnityEngine;

namespace Code.Manager
{
    public class ExternalLinkManager : MonoBehaviour
    {
        public void VlogLink()
        {
            Application.OpenURL("https://psb08.tistory.com/");
        }

        public void GithubLink()
        {
            Application.OpenURL("https://github.com/psb08");
        }
        
    }
}