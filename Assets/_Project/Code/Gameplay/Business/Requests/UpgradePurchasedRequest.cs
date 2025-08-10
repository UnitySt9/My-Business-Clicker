namespace Code.Gameplay.Business.Requests
{
    public struct UpgradePurchasedRequest
    {
        public readonly int BusinessId;
        public readonly int UpgradeId;

        public UpgradePurchasedRequest(int businessId, int upgradeId)
        {
            BusinessId = businessId;
            UpgradeId = upgradeId;
        }
    }
}