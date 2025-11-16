using System;

namespace Ingame
{
    public class PlayerSessionData
    {
        public uint uid;
        public uint worldPlayerUID;

        public string userName;

        public bool isHost;

        public DateTime joinedTime;
    }
}