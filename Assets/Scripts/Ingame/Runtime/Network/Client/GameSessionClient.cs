using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Ingame
{
    public class GameSessionClient : NetworkBehaviour
    {
        #region ========== Singleton ==========
        private static GameSessionClient _instance;

        public static GameSessionClient Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var found = FindObjectsOfType<GameSessionClient>();

                if (found.Length == 0)
                {
                    return null;
                }

                if (found.Length > 1)
                {
                    Debug.LogError($"[Singleton] Multiple instances of {typeof(GameSessionClient)} found in scene.");
                }

                _instance = found[0];
                return _instance;
            }
        }
        #endregion =========================

        [SerializeField]
        private PlayerSessionData sessionData;

        public void RequestJoin(string playerName)
        {
            RPC_RequestJoinServer(playerName);
        }

        [ServerRpc]
        private void RPC_RequestJoinServer(string playerName, NetworkConnection sender = null)
        {
            var sessionData = new PlayerSessionData()
            {
                userName = playerName
            };

            bool success = GameSessionSystem.Instance.JoinPlayer(sessionData);
            if (!success)
            {
                RPC_SendJoinFailedTarget(sender);
                return;
            }

            uint uid = sessionData.uid;
            RPC_SendJoinSuccessTarget(sender, uid);
        }

        [TargetRpc]
        private void RPC_SendJoinSuccessTarget(NetworkConnection connection, uint uid)
        {
            ClientSessionController.Instance.BindUID(uid);
        }

        [TargetRpc]
        private void RPC_SendJoinFailedTarget(NetworkConnection connection)
        {
            Debug.Log("Join Failed");
        }

        public void Join()
        {
            sessionData = new();
            bool success = GameSessionSystem.Instance.JoinPlayer(sessionData);

            if (!success)
            {
                Debug.LogError("Join failed.");
                return;
            }

            FocusClientPlayer(sessionData.worldPlayerUID);
        }

        private void FocusClientPlayer(uint uid)
        {
            ClientSessionController.Instance.BindUID(uid);
        }
    }
}
