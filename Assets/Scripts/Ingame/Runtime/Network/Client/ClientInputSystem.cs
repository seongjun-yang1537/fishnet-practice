using UnityEngine;
using UnityEngine.UIElements;

namespace Ingame
{
    public class ClientInputSystem : MonoBehaviour
    {
        [SerializeField]
        private uint currentUID = uint.MaxValue;
        private PlayerController currentPlayer => GetCurrentPlayerController();

        public void BindUID(uint uid)
        {
            this.currentUID = uid;
        }

        public void UnBindUID()
        {
            this.currentUID = uint.MaxValue;
        }

        protected virtual void Update()
        {
            if (currentUID == uint.MaxValue) return;

            Vector3 axisNormal = new Vector3(
                Input.GetAxis("Horizontal"),
                0f,
                Input.GetAxis("Vertical")
            );
            if (!Mathf.Approximately(axisNormal.sqrMagnitude, 0f))
                OnMove(axisNormal);

            if (Input.GetKeyDown(KeyCode.Space))
                OnJump();
        }

        private void OnMove(Vector3 normal)
        {
            currentPlayer?.Move(normal);
        }

        private void OnJump()
        {
            currentPlayer?.Jump();
        }

        private PlayerController GetCurrentPlayerController()
        {
            if (currentUID == uint.MaxValue) return null;
            return WorldController.Instance.FindPlayerByUID(currentUID);
        }
    }
}