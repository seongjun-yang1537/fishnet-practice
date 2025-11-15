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
            float golden = 0.618033988749895f;
            float hue = (uid * golden) % 1f;

            Color color = Color.HSVToRGB(hue, 0.8f, 1f);

            renderer.GetPropertyBlock(mpb);
            mpb.SetColor("_Color", color);
            renderer.SetPropertyBlock(mpb);
        }
    }
}
