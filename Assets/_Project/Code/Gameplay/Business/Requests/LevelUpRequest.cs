namespace Code.Gameplay.Business.Requests
{
    public struct LevelUpRequest
    {
        public readonly int BusinessId;
        public readonly int Level;

        public LevelUpRequest(int businessId, int level)
        {
            BusinessId = businessId;
            Level = level;
        }
    }
} 