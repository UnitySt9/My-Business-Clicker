using Code.Common;

namespace Code.Gameplay.Money
{
    public interface IMoneyService : IInitializable
    {
        void Set(int money);
        bool TryPurchase(int cost);
        void SetOwnerId(int id);
    }
}