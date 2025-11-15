namespace Ingame
{
    public class WorldModel
    {
        public string WorldIdentifier { get; set; } = string.Empty;

        public int ActiveEntityCount { get; set; }

        public float TimeScale { get; set; } = 1f;

        public bool IsPaused { get; set; }
    }
}
