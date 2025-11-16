using UnityEngine;

namespace Ingame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvasIngame;
        [SerializeField]
        private Canvas canvasOutgame;

        protected virtual void OnEnable()
        {
            GameSessionClient.Instance.onJoinGame.AddListener(OnJoinGame);
            GameSessionClient.Instance.onLeaveGame.AddListener(OnLeaveGame);
        }

        protected virtual void Start()
        {
            ShowOutgame();
        }

        protected virtual void OnDisable()
        {
            GameSessionClient.Instance.onJoinGame?.RemoveListener(OnJoinGame);
            GameSessionClient.Instance.onLeaveGame?.RemoveListener(OnLeaveGame);
        }

        public void ShowIngame()
        {
            canvasIngame.enabled = true;
            canvasOutgame.enabled = false;
        }

        public void ShowOutgame()
        {
            canvasIngame.enabled = false;
            canvasOutgame.enabled = true;
        }

        private void OnJoinGame()
        {
            ShowIngame();
        }

        private void OnLeaveGame()
        {
            ShowOutgame();
        }
    }
}