using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public class GameSessionClient : NetworkBehaviour
    {
        public static GameSessionClient Instance { get; private set; }

        public UnityEvent onJoinGame = new();
        public UnityEvent onLeaveGame = new();

        [SerializeField]
        private NetworkManager networkManager;

        private string currentPlayerName;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RequestHostJoin(string playerName)
        {
            currentPlayerName = playerName;
            ExecuteHostJoinAsync().Forget();
        }

        private async UniTaskVoid ExecuteHostJoinAsync()
        {
            networkManager.ServerManager.StartConnection();
            await UniTask.WaitUntil(() => networkManager.IsServerStarted);

            networkManager.ClientManager.StartConnection();
            await UniTask.WaitUntil(() => networkManager.IsClientStarted);

            LocalJoin();
        }

        public void RequestClientJoin(string playerName, string ipString)
        {
            currentPlayerName = playerName;
            ExecuteClientJoinAsync(ipString).Forget();
        }

        private async UniTaskVoid ExecuteClientJoinAsync(string ipString)
        {
            networkManager.TransportManager.Transport.SetClientAddress(ipString);
            networkManager.ClientManager.StartConnection();

            await UniTask.WaitUntil(() => networkManager.IsClientStarted);

            Debug.Log($"test: {networkManager.IsServerStarted}");
            RPC_RequestJoinServer(currentPlayerName);
        }

        [ServerRpc]
        private void RPC_RequestJoinServer(string playerName, NetworkConnection sender = null)
        {
            Debug.Log("RPC_RequestJoinServer");
            var sessionData = new PlayerSessionData { userName = playerName };
            if (!GameSessionSystem.Instance.JoinPlayer(sessionData))
            {
                RPC_SendJoinFailedTarget(sender);
                return;
            }

            RPC_SendJoinSuccessTarget(sender, sessionData.uid);
        }

        [TargetRpc]
        private void RPC_SendJoinSuccessTarget(NetworkConnection connection, uint uid)
        {
            ClientSessionController.Instance.BindUID(uid);
            Debug.Log("RPC_SendJoinSuccessTarget");
            onJoinGame.Invoke();
        }

        [TargetRpc]
        private void RPC_SendJoinFailedTarget(NetworkConnection connection)
        {
            Debug.Log("RPC_SendJoinFailedTarget");
            onLeaveGame.Invoke();
        }

        private void LocalJoin()
        {
            var sessionData = new PlayerSessionData { userName = currentPlayerName };
            if (!GameSessionSystem.Instance.JoinPlayer(sessionData))
            {
                onLeaveGame.Invoke();
                return;
            }

            ClientSessionController.Instance.BindUID(sessionData.worldPlayerUID);
            onJoinGame.Invoke();
        }

        protected virtual void Update()
        {
            if (networkManager.IsHostStarted)
            {
                Debug.Log("현재 이 인스턴스는 Host이다.");
            }

            if (networkManager.IsServerStarted && !networkManager.IsClientStarted)
            {
                Debug.Log("Dedicated Server로만 실행 중이다.");
            }

            if (networkManager.IsClientStarted && !networkManager.IsServerStarted)
            {
                Debug.Log("Remote Client로만 실행 중이다.");
            }
        }
    }
}
