using TriInspector;
using UnityEngine;

namespace Ingame
{
    public class ClientCameraController : MonoBehaviour
    {
        [Required]
        public CameraController cameraController;

        [SerializeField]
        private uint currentUID = uint.MaxValue;
        private PlayerController currentPlayer => GetCurrentPlayerController();

        public void BindUID(uint uid)
        {
            this.currentUID = uid;
            cameraController.FocusPlayer(currentPlayer);
        }

        public void UnBindUID()
        {
            this.currentUID = uint.MaxValue;
            cameraController.FocusPlayer(null);
        }

        private PlayerController GetCurrentPlayerController()
        {
            if (currentUID == uint.MaxValue) return null;
            return WorldController.Instance.FindPlayerByUID(currentUID);
        }
    }
}