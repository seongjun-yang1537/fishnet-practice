using TMPro;
using TriInspector;
using UnityEngine;

namespace Ingame
{
    public class OutgameUIController : MonoBehaviour
    {
        [Required, SerializeField]
        private TMP_InputField inputPlayerName;
        [Required, SerializeField]
        private TMP_InputField inputIpAddress;

        public void OnClickJoinHost()
            => OnClickJoinHost(inputPlayerName.text);

        public void OnClickJoinClient()
            => OnClickJoinClient(inputPlayerName.text, inputIpAddress.text);

        private void OnClickJoinHost(string playerName)
        {
            GameSessionClient.Instance.RequestHostJoin(playerName);
        }

        private void OnClickJoinClient(string playerName, string ipString)
        {
            GameSessionClient.Instance.RequestClientJoin(playerName, ipString);
        }
    }
}