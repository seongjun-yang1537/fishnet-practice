using System;
using System.Collections.Generic;

namespace Ingame
{
    [Serializable]
    public class WorldModel
    {
        public uint wid;
        public Dictionary<uint, PlayerModel> playerMap;

        public WorldModel()
        {
            playerMap = new();
        }

        public bool AddPlayer(PlayerModel playerModel)
        {
            return playerMap.TryAdd(playerModel.uid, playerModel);
        }

        public bool RemovePlayer(uint uid)
        {
            return playerMap.Remove(uid);
        }

        public bool RemovePlayer(PlayerModel playerModel)
            => RemovePlayer(playerModel.uid);
    }
}
