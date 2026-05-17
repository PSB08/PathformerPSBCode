using System;
using UnityEngine;

namespace Code.Object
{
    public class ViewSwitchShow : MonoBehaviour
    {
        [Tooltip("3D 모드에서 볼 수 있는가?")] public bool showableIn3D;
        
        private MeshRenderer objRenderer;
        
        private void Awake()
        {
            objRenderer = GetComponent<MeshRenderer>();
        }

        public void SetShowMode(bool is3DMode)
        {
            var material = objRenderer.material;

            if (is3DMode == showableIn3D)
            {
                SetAlpha(material, 1f);
            }
            else
            {
                SetAlpha(material, 0.2f);
            }
        }

        private void SetAlpha(Material mat, float alpha)
        {
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;
            
            mat.SetFloat("_Mode", 2);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
        
        
    }
}
