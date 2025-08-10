namespace Code.Gameplay.Money.Requests
{
    public readonly struct MoneyUpdateRequest
    {
        public readonly int OwnerId;
        public readonly int Value;
        
        public MoneyUpdateRequest(int ownerId, int value)
        {
            OwnerId = ownerId;
            Value = value;
        }
    }
}