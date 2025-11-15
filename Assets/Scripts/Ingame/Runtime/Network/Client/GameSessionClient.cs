using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Corelib.Utils;
using TriInspector;
using UnityEngine;

namespace Ingame
{
    public class GameSessionClient : Singleton<GameSessionClient>
    {
        [SerializeField]
        private PlayerSessionData sessionData;

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
            PlayerController pc = WorldController.Instance.FindPlayerByUID(uid);
            if (pc != null)
            {
                ClientSessionController.Instance.FocusTarget(pc);
            }
        }
    }
}
