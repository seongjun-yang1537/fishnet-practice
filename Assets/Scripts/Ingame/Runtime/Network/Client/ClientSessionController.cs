using System.Collections;
using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [RequireComponent(typeof(ClientInputSystem))]
    [RequireComponent(typeof(ClientCameraController))]
    public class ClientSessionController : Singleton<ClientSessionController>
    {
        #region ========== Event ==========
        public UnityEvent<PlayerController> onFocusPlayer = new();
        #endregion =========================

        [SerializeField]
        private uint currentUID = uint.MaxValue;
        private PlayerController currentPlayer => GetCurrentPlayerController();

        [SerializeField]
        private ClientInputSystem inputSystem;
        [SerializeField]
        private ClientCameraController cameraController;

        protected virtual void Awake()
        {
            inputSystem = GetComponent<ClientInputSystem>();
            cameraController = GetComponent<ClientCameraController>();
        }

        public void BindUID(uint uid)
        {
            this.currentUID = uid;
            OnFocusTarget();
        }

        public void UnBindUID()
        {
            this.currentUID = uint.MaxValue;
        }

        private void OnFocusTarget()
        {
            inputSystem.BindUID(currentUID);
            cameraController.BindUID(currentUID);

            onFocusPlayer.Invoke(currentPlayer);
        }

        private PlayerController GetCurrentPlayerController()
        {
            if (currentUID == uint.MaxValue) return null;
            return WorldController.Instance.FindPlayerByUID(currentUID);
        }
    }
}
