using UnityEngine;

namespace CleanRefactor
{
    public class ShopBootstrap : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private ShopView shopView;

        [Header("Shop Config Override")]
        [SerializeField] private int bombCost = 100;
        [SerializeField] private int shieldCost = 150;
        [SerializeField] private int doubleCoinsCost = 300;

        private void Awake()
        {
            // 1. Create native/domain layer models
            var config = new ShopConfig
            {
                BombCost = bombCost,
                ShieldCost = shieldCost,
                DoubleCoinsCost = doubleCoinsCost
            };

            // 2. Resolve structural implementations
            IPlayerRepository repository = new PlayerPrefsRepository();

            // 3. Construct Application Orchestrator UseCase
            var buyItemUseCase = new BuyItemUseCase(repository, config);

            // 4. Inject runtime domain into Presentation Controller
            if (shopView != null)
            {
                shopView.Initialize(buyItemUseCase);
            }
            else
            {
                Debug.LogError("ShopView missing from Composition Root configuration!");
            }
        }
    }
}