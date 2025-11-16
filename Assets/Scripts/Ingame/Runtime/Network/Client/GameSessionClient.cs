using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;

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

        #region ========== Event ==========
        public UnityEvent onJoinGame = new();
        public UnityEvent onLeaveGame = new();
        #endregion =========================

        [SerializeField]
        private NetworkManager networkManager;

        [SerializeField]
        private PlayerSessionData sessionData;

        private string currentPlayerName;

        protected virtual void OnEnable()
        {
            networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
        }

        protected virtual void OnDisable()
        {
            networkManager.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }

        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            if (args.ConnectionState == LocalConnectionState.Started)
            {
                RequestJoin(currentPlayerName);
            }
        }

        public void RequestHostJoin(string playerName)
        {
            this.currentPlayerName = playerName;
            ExecuteHost();
        }

        public void RequestClientJoin(string playerName, string ipString)
        {
            this.currentPlayerName = playerName;
            ExecuteClient(ipString);
        }

        public void RequestJoin(string playerName)
        {
            Debug.Log("RequestJoin");
            RPC_RequestJoinServer(playerName);
        }

        [ServerRpc]
        private void RPC_RequestJoinServer(string playerName, NetworkConnection sender = null)
        {
            Debug.Log("RPC_RequestJoinServer");

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
            Debug.Log("Join Success");
            onJoinGame.Invoke();
        }

        [TargetRpc]
        private void RPC_SendJoinFailedTarget(NetworkConnection connection)
        {
            Debug.Log("Join Failed");
            onLeaveGame.Invoke();
        }

        public void ExecuteHost()
        {
            networkManager.ServerManager.StartConnection();
            networkManager.ClientManager.StartConnection();
        }

        public void ExecuteClient(string ipString)
        {
            networkManager.TransportManager.Transport.SetClientAddress(ipString);
            networkManager.ClientManager.StartConnection();
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
