namespace Ingame
{
    public class PlayerSessionData
    {
        public string SessionIdentifier { get; set; } = string.Empty;

        public string PlayerName { get; set; } = string.Empty;

        public float AccumulatedPlayTime { get; set; }

        public bool IsConnected { get; set; }
    }
}
