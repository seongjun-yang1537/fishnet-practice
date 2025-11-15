using System;
using System.Collections.Generic;
using Corelib.Utils;
using TriInspector;
using UnityEngine;

namespace Ingame
{
    public class GameSessionSystem : Singleton<GameSessionSystem>
    {
        public static uint sessionUIDCounter = 0;

        [Required]
        public WorldController worldController;

        public Dictionary<uint, PlayerSessionData> sessionDatas = new();

        public bool JoinPlayer(PlayerSessionData sessionData)
        {
            uint newUID = sessionUIDCounter + 1;
            if (!sessionDatas.TryAdd(newUID, sessionData)) return false;

            PlayerController pc = worldController.SpawnPlayer();
            if (pc == null) return false;

            sessionData.uid = newUID;
            sessionData.joinedTime = DateTime.Now;
            sessionData.worldPlayerUID = pc.playerModel.uid;

            sessionUIDCounter++;

            return true;
        }

        public bool LeavePlayer(uint uid)
        {
            return sessionDatas.Remove(uid);
        }
        public bool LeavePlayer(PlayerSessionData sessionData)
            => LeavePlayer(sessionData.uid);
    }
}