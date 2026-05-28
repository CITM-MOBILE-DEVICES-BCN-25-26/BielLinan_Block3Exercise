namespace CleanRefactor
{
    public class BuyItemUseCase
    {
        private readonly IPlayerRepository _repository;
        private readonly ShopConfig _config;

        public BuyItemUseCase(IPlayerRepository repository, ShopConfig config)
        {
            _repository = repository;
            _config = config;
        }

        public ShopStatusDto GetCurrentStatus()
        {
            var player = _repository.Load();
            return new ShopStatusDto
            {
                CurrentCoins = player.Coins,
                CurrentLevel = player.Level,
                CanBuyBomb = player.Coins >= _config.BombCost && player.BombUses < _config.BombMaxUses,
                CanBuyShield = player.Coins >= _config.ShieldCost && player.ShieldUses < _config.ShieldMaxUses,
                CanBuyDoubleCoins = player.Coins >= _config.DoubleCoinsCost && player.Level >= _config.DoubleCoinsMinLevel && !player.HasDoubleCoins
            };
        }

        public void LevelUpPlayer()
        {
            var player = _repository.Load();
            player.Level++;
            _repository.Save(player);
        }

        public void AddCoins(int amount)
        {
            var player = _repository.Load();
            player.Coins += amount;
            _repository.Save(player);
        }

        public void ResetProfile()
        {
            var defaultPlayer = new PlayerState
            {
                Coins = 500,        
                Level = 1,          
                BombUses = 0,      
                ShieldUses = 0,   
                HasDoubleCoins = false 
            };
            _repository.Save(defaultPlayer);
        }
        public PurchaseStatus Execute(ShopItemType itemType)
        {
            var player = _repository.Load();

            switch (itemType)
            {
                case ShopItemType.Bomb:
                    if (player.Coins < _config.BombCost) return PurchaseStatus.NotEnoughCoins;
                    if (player.BombUses >= _config.BombMaxUses) return PurchaseStatus.MaxUsesReached;

                    player.Coins -= _config.BombCost;
                    player.BombUses++;
                    break;

                case ShopItemType.Shield:
                    if (player.Coins < _config.ShieldCost) return PurchaseStatus.NotEnoughCoins;
                    if (player.ShieldUses >= _config.ShieldMaxUses) return PurchaseStatus.MaxUsesReached;

                    player.Coins -= _config.ShieldCost;
                    player.ShieldUses++;
                    break;

                case ShopItemType.DoubleCoins:
                    if (player.Coins < _config.DoubleCoinsCost) return PurchaseStatus.NotEnoughCoins;
                    if (player.Level < _config.DoubleCoinsMinLevel) return PurchaseStatus.RequiredLevelNotReached;
                    if (player.HasDoubleCoins) return PurchaseStatus.AlreadyOwned;

                    player.Coins -= _config.DoubleCoinsCost;
                    player.HasDoubleCoins = true;
                    break;
            }

            _repository.Save(player);
            return PurchaseStatus.Purchased;
        }
    }
}