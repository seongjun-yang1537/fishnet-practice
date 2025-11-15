namespace Ingame
{
    public class PlayerModel
    {
        public string DisplayName { get; set; } = string.Empty;

        public int Level { get; set; }

        public float Health { get; set; } = 100f;

        public PlayerSessionData SessionData { get; set; } = new PlayerSessionData();

        public WorldModel World { get; set; } = new WorldModel();
    }
}
