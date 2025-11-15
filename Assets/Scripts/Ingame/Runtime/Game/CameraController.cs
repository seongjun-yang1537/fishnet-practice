using Cinemachine;
using UnityEngine;

namespace Ingame
{
    public class CameraController : MonoBehaviour
    {
        private Camera mainCamera;
        private CinemachineVirtualCamera vcam;

        protected virtual void Awake()
        {
            mainCamera = Camera.main;
            vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        private void FocusPlayer(Transform target)
        {
            if (vcam == null)
            {
                Debug.LogWarning("No CinemachineVirtualCamera found!");
                return;
            }

            vcam.Follow = target;
            vcam.LookAt = target;
        }

        public void FocusPlayer(PlayerController pc)
        {
            FocusPlayer(pc.transform);
        }
    }
}