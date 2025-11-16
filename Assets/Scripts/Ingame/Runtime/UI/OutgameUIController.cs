using UnityEngine;

namespace Ingame
{
    public class OutgameUIController : MonoBehaviour
    {
        [SerializeField]
        private GameSessionClient cachedSessionClient;

        private void Awake()
        {
            if (cachedSessionClient == null)
            {
                cachedSessionClient = GameSessionClient.Instance;
            }
        }

        public void OnClickJoinHost()
        {
            GameSessionClient targetClient = cachedSessionClient != null ? cachedSessionClient : GameSessionClient.Instance;
            if (targetClient == null)
            {
                Debug.LogError("GameSessionClient is not available.");
                return;
            }

            targetClient.Join();
        }
    }
}
