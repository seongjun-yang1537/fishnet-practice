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
        public UnityEvent<PlayerController> onFocusPlayer = new();

        [SerializeField]
        private ClientInputSystem inputSystem;
        [SerializeField]
        private ClientCameraController cameraController;

        [SerializeField]
        private PlayerController currentPlayer;

        protected virtual void Awake()
        {
            inputSystem = GetComponent<ClientInputSystem>();
            cameraController = GetComponent<ClientCameraController>();
        }

        public void FocusTarget(PlayerController pc)
        {
            this.currentPlayer = pc;

            inputSystem.BindUID(pc.playerModel.uid);
            cameraController.BindUID(pc.playerModel.uid);

            onFocusPlayer.Invoke(pc);
        }
    }
}
