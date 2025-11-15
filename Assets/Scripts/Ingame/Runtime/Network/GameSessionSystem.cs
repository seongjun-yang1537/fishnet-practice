using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class GameSessionSystem : MonoBehaviour
    {
        private static uint sessionUidCounter = 0;

        public Dictionary<uint, PlayerSessionData> sessionDatas = new();

        public bool JoinPlayer(PlayerSessionData sessionData)
        {
            uint newUid = sessionUidCounter++;

            sessionData.uid = newUid;
            sessionData.joinedTime = DateTime.Now;

            return sessionDatas.TryAdd(newUid, sessionData);
        }

        public bool LeavePlayer(uint uid)
        {
            return sessionDatas.Remove(uid);
        }
        public bool LeavePlayer(PlayerSessionData sessionData)
            => LeavePlayer(sessionData.uid);
    }
}