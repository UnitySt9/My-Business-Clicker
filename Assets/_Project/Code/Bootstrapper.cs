using System.Collections.Generic;
using Code.Common.Services;
using Code.Gameplay.Business;
using Code.Gameplay.Business.Configs;
using Code.Gameplay.Business.Features;
using Code.Gameplay.Hero.Features;
using Code.Gameplay.Money;
using Code.Gameplay.Money.Features;
using Code.Gameplay.Save;
using Code.UI;
using Code.UI.Money;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Transform _mainUi;
        [SerializeField] private Transform _businessUIParent;
        [SerializeField] private MoneyView _moneyView;

        private readonly List<IUIModel> _uiModels = new(10);
        
        private EcsWorld _world;
        private IEcsSystems _systems;
        private IIdentifierService _identifierService;
        private ISaveService _saveService;
        private ITimeService _timeService;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _identifierService = new IdentifierService();
            _saveService = new PlayerPrefsSaveService();
            _timeService = new UnityTimeService();

            StaticDataService staticData = LoadStaticData();
            BusinessUpgradeNamesConfig businessUpgradeNamesConfig = staticData.GetBusinessUpgradeNamesConfig();
            BusinessConfig businessConfig = staticData.GetBusinessConfig();

            CurrencyModel currencyModel = new CurrencyModel();

            IMoneyService moneyService = new HeroMoneyService(currencyModel, _world);
            moneyService.Initialize();

            BusinessService businessService = new BusinessService(_world, moneyService);
            businessService.Initialize();

            _moneyView.Initialize(new CurrencyScreenModel(currencyModel));

            CreateBusinessViews(businessConfig.GetBusinessDatas(), staticData, businessService,
                businessUpgradeNamesConfig);

            InitFeatures(businessService, moneyService, businessUpgradeNamesConfig, businessConfig);
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems = null;
            _world?.Destroy();
            _world = null;
            _uiModels.ForEach(x => x.Dispose());
        }

        private void InitFeatures(BusinessService businessService, IMoneyService heroMoneyService,
            BusinessUpgradeNamesConfig businessUpgradeNamesConfig, BusinessConfig businessConfig)
        {
            var heroFeature = new HeroFeature(_world, _systems, _identifierService, _saveService);
            var businessFeature = new BusinessFeature(_world,
                    _systems,
                    businessService,
                    _identifierService,
                    businessUpgradeNamesConfig,
                    businessConfig, _saveService, _timeService);
            
            var moneyFeature = new MoneyFeature(_world, _systems, heroMoneyService);
            var saveFeature = new SaveFeature(_world, _systems, _saveService, _timeService);

            heroFeature.RegisterSystems();
            businessFeature.RegisterSystems();
            moneyFeature.RegisterSystems();
            saveFeature.RegisterSystems();

            _systems.Init();
        }

        private void CreateBusinessViews(IReadOnlyList<BusinessData> businessDatas, StaticDataService staticData,
            BusinessService businessService, BusinessUpgradeNamesConfig businessUpgradeNamesConfig)
        {
            int lastBusinessId = 0;

            for (int i = 0; i < businessDatas.Count; i++)
            {
                BusinessView
                    businessView = Instantiate(staticData.GetBusinessView, _businessUIParent); //todo: move to factory

                BusinessData businessData = businessDatas[i];

               var upgradeBusinessScreenModels = CreateUpgradeScreenModels(businessUpgradeNamesConfig, i, businessData, businessService);

               _uiModels.AddRange(upgradeBusinessScreenModels);
               
                businessView.Initialize(new BusinessScreenModel(businessService, lastBusinessId++, upgradeBusinessScreenModels));
            }
        }

        private List<UpgradeBusinessScreenModel> CreateUpgradeScreenModels(
            BusinessUpgradeNamesConfig businessUpgradeNamesConfig, int businessId, BusinessData businessData,
            BusinessService businessService)
        {
            List<UpgradeBusinessScreenModel> upgradeBusinessScreenModels = new List<UpgradeBusinessScreenModel>();
            List<string> upgradeNames = businessUpgradeNamesConfig.BusinessUpgradeNameDatas[businessId].UpgradeNames;

            for (int i = 0; i < businessData.Upgrades.Length; i++)
            {
                string targetName = upgradeNames[i];
                bool purchased = businessData.Upgrades[i].Purchased;
                int cost = businessData.Upgrades[i].Cost;
                float incomeMultiplier = businessData.Upgrades[i].IncomeMultiplier;
                
                UpgradeBusinessScreenModel upgradeBusinessScreenModel = new(
                    purchased, 
                    incomeMultiplier, 
                    cost, 
                    targetName, 
                    i, 
                    businessId, 
                    businessService
                );
        
                upgradeBusinessScreenModels.Add(upgradeBusinessScreenModel);
                _uiModels.Add(upgradeBusinessScreenModel);
            }

            return upgradeBusinessScreenModels;
        }

        private static StaticDataService LoadStaticData()
        {
            var staticDataService = new StaticDataService();

            staticDataService.LoadAll();

            return staticDataService;
        }
    }
}