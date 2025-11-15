using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public class PlayerView : MonoBehaviour
    {
        public UnityEvent<uint> onChangedColor = new();

        public MeshRenderer renderer;
        MaterialPropertyBlock mpb;

        protected virtual void Awake()
        {
            mpb = new MaterialPropertyBlock();
        }

        protected virtual void OnEnable()
        {
            onChangedColor.AddListener(OnChangedColor);
        }

        protected virtual void OnDisable()
        {
            onChangedColor.RemoveListener(OnChangedColor);
        }

        private void OnChangedColor(uint uid)
        {
            float hue = (uid % 1000) / 1000f;
            Color color = Color.HSVToRGB(hue, 0.8f, 1f);

            renderer.GetPropertyBlock(mpb);
            mpb.SetColor("_BaseColor", color);
            renderer.SetPropertyBlock(mpb);
        }
    }
}
