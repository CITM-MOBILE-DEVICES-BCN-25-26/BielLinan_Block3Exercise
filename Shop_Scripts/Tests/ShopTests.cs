using NUnit.Framework;

namespace CleanRefactor.Tests
{
    [TestFixture]
    public class ShopTests
    {
        private MockPlayerRepository _mockRepository;
        private ShopConfig _config;
        private BuyItemUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockPlayerRepository();
            _config = new ShopConfig();
            _useCase = new BuyItemUseCase(_mockRepository, _config);
        }

        [Test]
        public void BombPurchase_Succeeds_WhenConditionsAreMet()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 150, BombUses = 0 };

            // Act
            var result = _useCase.Execute(ShopItemType.Bomb);

            // Assert
            Assert.AreEqual(PurchaseStatus.Purchased, result);
            Assert.AreEqual(1, _mockRepository.State.BombUses);
        }

        [Test]
        public void BombPurchase_Fails_WhenPlayerDoesNotHaveEnoughCoins()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 50, BombUses = 0 };

            // Act
            var result = _useCase.Execute(ShopItemType.Bomb);

            // Assert
            Assert.AreEqual(PurchaseStatus.NotEnoughCoins, result);
        }

        [Test]
        public void BombPurchase_Fails_WhenMaximumUsesAreReached()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 500, BombUses = 3 };

            // Act
            var result = _useCase.Execute(ShopItemType.Bomb);

            // Assert
            Assert.AreEqual(PurchaseStatus.MaxUsesReached, result);
        }

        [Test]
        public void ShieldPurchase_Succeeds_WhenConditionsAreMet()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 200, ShieldUses = 0 };

            // Act
            var result = _useCase.Execute(ShopItemType.Shield);

            // Assert
            Assert.AreEqual(PurchaseStatus.Purchased, result);
            Assert.AreEqual(1, _mockRepository.State.ShieldUses);
        }

        [Test]
        public void ShieldPurchase_Fails_WhenMaximumUsesAreReached()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 500, ShieldUses = 2 };

            // Act
            var result = _useCase.Execute(ShopItemType.Shield);

            // Assert
            Assert.AreEqual(PurchaseStatus.MaxUsesReached, result);
        }

        [Test]
        public void DoubleCoinsPurchase_Succeeds_WhenCoinsAreEnough_AndLevelIsFiveOrHigher()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 400, Level = 5, HasDoubleCoins = false };

            // Act
            var result = _useCase.Execute(ShopItemType.DoubleCoins);

            // Assert
            Assert.AreEqual(PurchaseStatus.Purchased, result);
            Assert.IsTrue(_mockRepository.State.HasDoubleCoins);
        }

        [Test]
        public void DoubleCoinsPurchase_Fails_WhenPlayerLevelIsLowerThanFive()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 400, Level = 4, HasDoubleCoins = false };

            // Act
            var result = _useCase.Execute(ShopItemType.DoubleCoins);

            // Assert
            Assert.AreEqual(PurchaseStatus.RequiredLevelNotReached, result);
        }

        [Test]
        public void DoubleCoinsPurchase_Fails_WhenAlreadyOwned()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 400, Level = 6, HasDoubleCoins = true };

            // Act
            var result = _useCase.Execute(ShopItemType.DoubleCoins);

            // Assert
            Assert.AreEqual(PurchaseStatus.AlreadyOwned, result);
        }

        [Test]
        public void PlayerCoins_AreUpdated_AfterSuccessfulPurchase()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 500, BombUses = 0 };

            // Act
            _useCase.Execute(ShopItemType.Bomb);

            // Assert
            Assert.AreEqual(400, _mockRepository.State.Coins);
        }

        [Test]
        public void PlayerSaved_OnlyWhenPurchaseSucceeds_SuccessCase()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 200, BombUses = 0 };
            _mockRepository.ResetSaveFlag();

            // Act
            _useCase.Execute(ShopItemType.Bomb);

            // Assert
            Assert.IsTrue(_mockRepository.SaveWasCalled);
        }

        [Test]
        public void PlayerSaved_OnlyWhenPurchaseSucceeds_FailureCase()
        {
            // Arrange
            _mockRepository.State = new PlayerState { Coins = 20, BombUses = 0 };
            _mockRepository.ResetSaveFlag();

            // Act
            _useCase.Execute(ShopItemType.Bomb);

            // Assert
            Assert.IsFalse(_mockRepository.SaveWasCalled);
        }
    }
}